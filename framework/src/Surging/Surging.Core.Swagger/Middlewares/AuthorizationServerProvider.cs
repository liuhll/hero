using Surging.Core.CPlatform.Routing;
using Surging.Core.ProxyGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Core.Swagger.Middlewares
{
    public class AuthorizationServerProvider : IAuthorizationServerProvider
    {
        private readonly IServiceProxyProvider _serviceProxyProvider;//务代理
        private readonly IServiceRouteProvider _serviceRouteProvider;//用务路由
        //private readonly IJwtTokenProvider _jwtTokenProvider;

        /// <summary>
        /// 安全验证服务提供类构造
        /// </summary>
        /// <param name="serviceProxyProvider">服务代理</param>
        /// <param name="serviceRouteProvider">用务路由</param>
        public AuthorizationServerProvider(IServiceProxyProvider serviceProxyProvider
           , IServiceRouteProvider serviceRouteProvider
            /*IJwtTokenProvider jwtTokenProvider*/)
        {
            _serviceProxyProvider = serviceProxyProvider;
            _serviceRouteProvider = serviceRouteProvider;
            //  _jwtTokenProvider = jwtTokenProvider;

        }

        public async Task<bool> Authorize(string apiPath, Dictionary<string, object> parameters)
        {
            var route = await _serviceRouteProvider.GetRouteByPath(apiPath);

            //if (AppConfig.WhiteList.Contains(apiPath))
            //{
            //    return true;
            //}

            //if (route.ServiceDescriptor.AllowPermission())
            //{
            //    return true;
            //}

            //return await _serviceProxyProvider.Invoke<bool>(parameters, AppConfig.SwaggerOptions.Authorization.AuthorizationRoutePath,
            //    AppConfig.SwaggerOptions.Authorization.AuthorizationServiceKey ?? "");
            return false;
        }

        public IDictionary<string, object> GetPayload(string token)
        {
            //return _jwtTokenProvider.GetPayLoad(token, AppConfig.SwaggerOptions.Authorization.SecretKey);
            return null;
        }
    }
}
