using System;

namespace Surging.Core.ApiGateWay.OAuth
{
    public class ConfigInfo
    {
        public ConfigInfo(string authorizationRoutePath) : this(authorizationRoutePath, null, null, TimeSpan.FromMinutes(30))
        {
        }

        public ConfigInfo(string authorizationRoutePath, string authorizationServiceKey, string authenticationServiceKey, TimeSpan accessTokenExpireTimeSpan)
        {
            AuthorizationServiceKey = authorizationServiceKey;
            AuthorizationRoutePath = authorizationRoutePath;
            AuthenticationServiceKey = authenticationServiceKey;
            AccessTokenExpireTimeSpan = accessTokenExpireTimeSpan;
        }

        public string AuthorizationServiceKey { get; set; }

        public string AuthenticationServiceKey { get; set; }
        public string AuthorizationRoutePath { get; set; }
        public TimeSpan AccessTokenExpireTimeSpan { get; set; } = TimeSpan.FromMinutes(30);
    };
}