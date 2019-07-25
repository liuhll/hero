using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Surging.Core.ApiGateWay;
using Surging.Core.ApiGateWay.OAuth;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Serialization;
using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;
using GateWayAppConfig = Surging.Core.ApiGateWay.AppConfig;
using MessageStatusCode = Surging.Core.CPlatform.Messages.StatusCode;

namespace Hl.Gateway.WebApi.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceProxyProvider _serviceProxyProvider;
        private readonly IServiceRouteProvider _serviceRouteProvider;
        private readonly IAuthorizationServerProvider _authorizationServerProvider;
        private readonly IServicePartProvider _servicePartProvider;
        private readonly ISerializer<string> _serializer;

        public ServicesController(IServiceProxyProvider serviceProxyProvider,
            IServiceRouteProvider serviceRouteProvider,
            IAuthorizationServerProvider authorizationServerProvider,
            IServicePartProvider servicePartProvider,
            ISerializer<string> serializer)
        {
            _serviceProxyProvider = serviceProxyProvider;
            _serviceRouteProvider = serviceRouteProvider;
            _authorizationServerProvider = authorizationServerProvider;
            _servicePartProvider = servicePartProvider;
            _serializer = serializer;
        }

        public async Task<ServiceResult<object>> Path([FromServices]IServicePartProvider servicePartProvider, string path, [FromBody]Dictionary<string, object> model)
        {
            var serviceKey = this.Request.Query["servicekey"];
            var rpcParams = new Dictionary<string, object>();
            switch (Request.Method)
            {
                case "GET":
                    
                    foreach (string n in this.Request.Query.Keys)
                    {
                        rpcParams[n] = this.Request.Query[n].ToString();
                    }
                    break;
                case "POST":
                    if (model == null || !model.Any())
                    {
                        return new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.RequestError, Message = $"请使用GET方法重试" };
                    }
                    rpcParams = model;
                    break;
                default:
                    return new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.RequestError, Message = $"暂不支持{Request.Method}请求方法" };

            }
          
           
            var result = ServiceResult<object>.Create(false, null);
            path = path.ToLower();
            if (await GetAllowRequest(path) == false)
            {
                return new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.RequestError, Message = "请求错误" };
            }
            var appConfig = GateWayAppConfig.ServicePart;

            if (servicePartProvider.IsPart(path))
            {
                var data = (string)await servicePartProvider.Merge(path, rpcParams);
                return CreateServiceResult(data);
            }
            else
            {
                if (path == GateWayAppConfig.AuthenticationRoutePath)
                {
                    try
                    {
                        var token = await _authorizationServerProvider.GenerateTokenCredential(rpcParams);
                        if (token != null)
                        {
                            result = ServiceResult<object>.Create(true, token);
                            result.StatusCode = MessageStatusCode.OK;
                        }
                        else
                        {
                            result = new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.UnAuthentication, Message = "不合法的身份凭证" };
                        }
                    }
                    catch (CPlatformException ex)
                    {
                        result = new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.CPlatformError, Message = ex.Message };
                    }
                    catch (Exception ex)
                    {
                        result = new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.UnKnownError, Message = ex.Message };
                    }
                }
                else
                {
                    if (OnAuthorization(path, rpcParams, ref result))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(serviceKey))
                            {
                                var data = await _serviceProxyProvider.Invoke<object>(rpcParams, path, serviceKey);
                                return CreateServiceResult(data);
                            }
                            else
                            {
                                var data = await _serviceProxyProvider.Invoke<object>(rpcParams, path);
                                if (data == null)
                                {
                                    return new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.UnKnownError, Message = "服务异常" };
                                }
                                return CreateServiceResult(data);
                            }
                        }
                        catch (CPlatformException ex)
                        {
                            return new ServiceResult<object> { IsSucceed = false, StatusCode = ex.ExceptionCode, Message = ex.Message };
                        }
                        catch (Exception ex)
                        {
                            return new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.UnKnownError, Message = ex.Message };
                        }
                    }

                }

            }

            return result;
        }

        private async Task<bool> GetAllowRequest(string path)
        {

            var route = await _serviceRouteProvider.GetRouteByPath(path);
            return !route.ServiceDescriptor.DisableNetwork();
        }
        private bool OnAuthorization(string path, Dictionary<string, object> model, ref ServiceResult<object> result)
        {
            bool isSuccess = false;
            var route = _serviceRouteProvider.GetRouteByPath(path).Result;
            if (route.ServiceDescriptor.EnableAuthorization())
            {
                isSuccess = route.ServiceDescriptor.AuthType() == AuthorizationType.JWT.ToString()
                    ? ValidateJwtAuthentication(route, model, ref result) : ValidateAppSecretAuthentication(route, path, model, ref result);
            }

            //if (isSuccess)
            //{
            //    if (path != GateWayAppConfig.AuthenticationRoutePath && 
            //        path != GateWayAppConfig.ThirdPartyAuthenticationRoutePath)
            //    {
            //        var authParams = new Dictionary<string, object>()
            //        {
            //            {
            //                "input", new
            //                {
            //                    Path = path
            //                }
            //            }
            //        };
            //        isSuccess = _authorizationServerProvider.Authorize(path, authParams).Result;
            //        if (!isSuccess)
            //        {
            //            result = new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.UnAuthorized, Message = $"您没有访问{path}接口的权限" };
            //        }
            //    }
            //}
            return isSuccess;
        }

        private bool ValidateAppSecretAuthentication(ServiceRoute route, string path, Dictionary<string, object> model, ref ServiceResult<object> result)
        {
            bool isSuccess = true;
            DateTime time;
            var author = HttpContext.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(path) && model.ContainsKey("timeStamp") && author.Count > 0)
            {
                if (DateTime.TryParse(model["timeStamp"].ToString(), out time))
                {
                    var seconds = (DateTime.Now - time).TotalSeconds;
                    if (seconds <= 3560 && seconds >= 0) //:todo 配置tOKen的有效期
                    {
                        if (GetMD5($"{route.ServiceDescriptor.Token}{time.ToString("yyyy-MM-dd hh:mm:ss") }") != author.ToString())
                        {
                            result = new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.UnAuthentication, Message = "不合法的身份凭证" };
                            isSuccess = false;
                        }
                    }
                    else
                    {
                        result = new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.UnAuthentication, Message = "不合法的身份凭证" };
                        isSuccess = false;
                    }
                }
                else
                {
                    result = new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.UnAuthentication, Message = "不合法的身份凭证" };
                    isSuccess = false;
                }
            }
            else
            {
                result = new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.RequestError, Message = "不合法的身份凭证" };
                isSuccess = false;
            }
            return isSuccess;
        }

        public bool ValidateJwtAuthentication(ServiceRoute route, Dictionary<string, object> model, ref ServiceResult<object> result)
        {
            bool isSuccess = true;
            var author = HttpContext.Request.Headers["Authorization"].ToString();
            if (author.StartsWith("Bearer "))
            {
                author = author.Substring(7);
            }
            if (!string.IsNullOrEmpty(author))
            {
                isSuccess = _authorizationServerProvider.ValidateClientAuthentication(author).Result;
                if (!isSuccess)
                {
                    result = new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.UnAuthentication, Message = "不合法的身份凭证" };
                }
                else
                {
                    var keyValue = model.FirstOrDefault();
                    if (!(keyValue.Value is IConvertible) || !typeof(IConvertible).GetTypeInfo().IsAssignableFrom(keyValue.Value.GetType()))
                    {
                        var payload = _authorizationServerProvider.GetPayLoad(author);
                        RpcContext.GetContext().SetAttachment("payload", payload);
                    }
                }
            }
            else
            {
                result = new ServiceResult<object> { IsSucceed = false, StatusCode = MessageStatusCode.UnAuthentication, Message = "请先登录系统" };
                isSuccess = false;
            }
            return isSuccess;
        }

        public static string GetMD5(string encypStr)
        {
            try
            {
                var md5 = MD5.Create();
                var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(encypStr));
                var sb = new StringBuilder();
                foreach (byte b in bs)
                {
                    sb.Append(b.ToString("X2"));
                }
                //所有字符转为大写
                return sb.ToString().ToLower();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                return null;
            }
        }

        private ServiceResult<object> CreateServiceResult(object data)
        {
            if (data.GetType() == typeof(string))
            {
                var dataStr = (string)data;
                if (dataStr.IsValidJson())
                {
                    var serializer = ServiceLocator.GetService<ISerializer<string>>();
                    var dataObj = serializer.Deserialize(dataStr, typeof(object), true);
                    var serviceResult = ServiceResult<object>.Create(true, dataObj);
                    serviceResult.StatusCode = MessageStatusCode.OK;
                    return serviceResult;
                }
                else
                {
                    var serviceResult = ServiceResult<object>.Create(true, data);
                    serviceResult.StatusCode = MessageStatusCode.OK;
                    return serviceResult;

                }
            }
            else
            {
                var serviceResult = ServiceResult<object>.Create(true, data);
                serviceResult.StatusCode = MessageStatusCode.OK;
                return serviceResult;
            }
        }
    }
}
