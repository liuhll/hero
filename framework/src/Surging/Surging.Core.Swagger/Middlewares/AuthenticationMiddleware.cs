using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Jwt;
using Surging.Core.CPlatform.Messages;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Serialization;
using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.KestrelHttpServer.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Core.Swagger.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISerializer<string> _jsonSerializer;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
            _jsonSerializer = ServiceLocator.GetService<ISerializer<string>>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await OnAuthorize(context);
                await _next(context);
            }
            catch (AuthenticationException ex)
            {
                var noAuthResponseContent = new HttpResultMessage()
                { IsSucceed = false, Message = ex.Message, StatusCode = StatusCode.UnAuthentication };
                await context.Response.WriteAsync(_jsonSerializer.Serialize(noAuthResponseContent, true));
            }
            catch (CPlatformException ex)
            {
                var resultMessage = new HttpResultMessage()
                { IsSucceed = false, Message = ex.Message, StatusCode = StatusCode.CPlatformError };
                await context.Response.WriteAsync(_jsonSerializer.Serialize(resultMessage, true));
            }
            catch (Exception ex)
            {
                var resultMessage = new HttpResultMessage()
                { IsSucceed = false, Message = ex.StackTrace, StatusCode = StatusCode.RequestError };
                await context.Response.WriteAsync(_jsonSerializer.Serialize(resultMessage, true));
            }

        }

        private async Task OnAuthorize(HttpContext context)
        {
            try
            {
                var serviceRouteProvider = ServiceLocator.GetService<IServiceRouteProvider>();
                var routPath = GetRoutePath(context.Request.Path.ToString());
                var commandInfo = await serviceRouteProvider.GetRouteByPath(routPath);
                if (commandInfo == null)
                {
                    throw new CPlatformException($"系统中不存在{routPath}的路由信息", StatusCode.CPlatformError);
                }

                if (!commandInfo.ServiceDescriptor.EnableAuthorization())
                {
                    return;
                }

                var token = context.Request.GetTokenFromHeader();
                var jwtTokenProvider = ServiceLocator.GetService<IJwtTokenProvider>();
                var isSuccess = jwtTokenProvider.ValidateToken(token, AppConfig.SwaggerOptions.Authorization.SecretKey);

                if (isSuccess)
                {
                    if (AppConfig.SwaggerOptions.Authorization.EnableAuthorization && commandInfo.ServiceDescriptor.EnableAuthorization())
                    {
                       await SetRpcContextPayload(context);
                        
                    }
                    if (!string.IsNullOrEmpty(AppConfig.SwaggerOptions.Authorization.AuthorizationRoutePath))
                    {
                        var apiPath = context.Request.Path.ToString().TrimStart('/');
                        var authorizationServerProvider = ServiceLocator.GetService<IAuthorizationServerProvider>();
                        await authorizationServerProvider.Authorize(AppConfig.SwaggerOptions.Authorization.AuthorizationRoutePath, new Dictionary<string, object>()
                        {
                            {"apiPath", apiPath}
                        });
                    }
                }
                else
                {
                    throw new AuthenticationException("不合法的身份凭证");
                }
            }
            catch (Exception ex)
            {
                throw new AuthenticationException(ex.Message);
            }
        }

        private async Task SetRpcContextPayload(HttpContext context)
        {
            if (context.Request.Method.ToUpper() == "POST")
            {
                var authorizationServerProvider = ServiceLocator.GetService<IAuthorizationServerProvider>();
                var payload = authorizationServerProvider.GetPayload(context.Request.GetTokenFromHeader());
                RpcContext.GetContext().SetAttachment("payload", _jsonSerializer.Serialize(payload, true));
            }
            else
            {
                var authorizationServerProvider = ServiceLocator.GetService<IAuthorizationServerProvider>();
                var payload = authorizationServerProvider.GetPayload(context.Request.GetTokenFromHeader());
                RpcContext.GetContext().SetAttachment("payload", _jsonSerializer.Serialize(payload, true));
            }
            
        }

        private string GetRoutePath(string path)
        {
            string routePath = "";
            var urlSpan = path.AsSpan();

            var len = urlSpan.IndexOf("?");
            if (len == -1)
                routePath = urlSpan.TrimStart("/").ToString().ToLower();
            else
                routePath = urlSpan.Slice(0, len).TrimStart("/").ToString().ToLower();
            return routePath;
        }
    }

    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthentication(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
