using Surging.Core.ApiGateWay.OAuth.Models;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Jwt;
using Surging.Core.CPlatform.Routing;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Surging.Core.ApiGateWay.OAuth
{
    /// <summary>
    /// 安全验证服务提供类
    /// </summary>
    public class AuthorizationServerProvider : IAuthorizationServerProvider
    {
        private readonly IServiceProxyProvider _serviceProxyProvider;//务代理
        private readonly IServiceRouteProvider _serviceRouteProvider;//用务路由
        private readonly IJwtTokenProvider _jwtTokenProvider;
        /// <summary>
        /// 安全验证服务提供类构造
        /// </summary>
        /// <param name="configInfo">配置信息</param>
        /// <param name="serviceProxyProvider">服务代理</param>
        /// <param name="serviceRouteProvider">用务路由</param>
        /// <param name="jwtTokenProvider">jwt token provider</param>
        public AuthorizationServerProvider(
            IServiceProxyProvider serviceProxyProvider,
            IServiceRouteProvider serviceRouteProvider,
            IJwtTokenProvider jwtTokenProvider)
        {
            _serviceProxyProvider = serviceProxyProvider;
            _serviceRouteProvider = serviceRouteProvider;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<string> GenerateTokenCredential(IDictionary<string, object> rpcParams)
        {
            LoginResult loginResult;
            if (AppConfig.AuthorizationServiceKey.IsNullOrEmpty())
            {
                loginResult = await _serviceProxyProvider.Invoke<LoginResult>(rpcParams, AppConfig.AuthenticationRoutePath);
            }
            else
            {
                loginResult = await _serviceProxyProvider.Invoke<LoginResult>(rpcParams, AppConfig.AuthenticationRoutePath, AppConfig.AuthenticationServiceKey);
            }
            if (loginResult == null)
            {
                throw new BusinessException("当前系统无法登陆,请稍后重试");
            }
            if (loginResult.ResultType == LoginResultType.Fail)
            {
                throw new AuthException(loginResult.ErrorMessage);
            }
            if (loginResult.ResultType == LoginResultType.Error)
            {
                throw new BusinessException(loginResult.ErrorMessage);
            }
            var jwtHader = new Dictionary<string, object>() {
                { "timeStamp", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { "typ", "JWT" },
                { "alg", AppConfig.JwtConfig.EncryptionAlgorithm }
            };
            var payload = loginResult.PayLoad;
            payload.Add("iss", AppConfig.JwtConfig.Iss);
            payload.Add("aud", AppConfig.JwtConfig.Aud);
            payload.Add("iat", DateTime.Now);
            payload.Add("exp", DateTime.Now.AddMinutes(AppConfig.JwtConfig.Period));
            //payload.Add("ast", accessSystemType);
            return _jwtTokenProvider.GenerateToken(jwtHader, payload, AppConfig.JwtConfig.SecretKey, AppConfig.JwtConfig.EncryptionAlgorithm);
        }


        public async Task<bool> Authorize(string apiPath, Dictionary<string, object> parameters)
        {
            var route = await _serviceRouteProvider.GetRouteByPath(apiPath);

            if (AppConfig.WhiteList.Contains(apiPath))
            {
                return true;
            }

            if (route.ServiceDescriptor.AllowPermission())
            {
                return true;
            }

            return await _serviceProxyProvider.Invoke<bool>(parameters, AppConfig.AuthorizationRoutePath,
                AppConfig.AuthorizationServiceKey);
        }

        /// <summary>
        /// 得到TOKEN内容，token包含三部份（头/内容/验证密文）
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public IDictionary<string, object> GetPayLoad(string token)
        {
            return _jwtTokenProvider.GetPayLoad(token, AppConfig.JwtConfig.SecretKey);
        }


        /// <summary>
        /// 身份认证
        /// </summary>
        /// <param name="token">客户端TOKEN值</param>
        /// <returns></returns>
        public async Task<bool> ValidateClientAuthentication(string token)
        {
            return _jwtTokenProvider.ValidateToken(token, AppConfig.JwtConfig.SecretKey);
        }
    }
}