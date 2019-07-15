using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Surging.Core.ApiGateWay.Configurations;
using Surging.Core.ApiGateWay.OAuth.Implementation.Configurations;
using Surging.Core.CPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.ApiGateWay
{
    public static class AppConfig
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static string _authorizationServiceKey;

        public static string AuthorizationServiceKey
        {
            get
            {
                return Configuration["AuthorizationServiceKey"] ?? _authorizationServiceKey;
            }
            internal set
            {
                _authorizationServiceKey = value;
            }
        }

        private static string _authenticationServiceKey;

        public static string AuthenticationServiceKey
        {
            get
            {
                return Configuration["AuthenticationServiceKey"] ?? _authenticationServiceKey;
            }
            internal set
            {
                _authenticationServiceKey = value;
            }
        }

        private static string _authorizationRoutePath;

        public static string AuthorizationRoutePath
        {
            get
            {
                return Configuration["AuthorizationRoutePath"] ?? _authorizationRoutePath;
            }
            internal set
            {
                _authorizationRoutePath = value;
            }
        }

        private static TimeSpan _accessTokenExpireTimeSpan = TimeSpan.FromMinutes(30);

        public static TimeSpan AccessTokenExpireTimeSpan
        {
            get
            {
                int tokenExpireTime;
                if (Configuration["AccessTokenExpireTimeSpan"] != null && int.TryParse(Configuration["AccessTokenExpireTimeSpan"], out tokenExpireTime))
                {
                    _accessTokenExpireTimeSpan = TimeSpan.FromMinutes(tokenExpireTime);
                }
                return _accessTokenExpireTimeSpan;
            }
            internal set
            {
                _accessTokenExpireTimeSpan = value;
            }
        }

        private static string _authenticationRoutePath = "oauth2/token";

        public static string AuthenticationRoutePath
        {
            get
            {
                return Configuration["AuthenticationRoutePath"] ?? _authenticationRoutePath;
            }
            internal set
            {
                _authenticationRoutePath = value;
            }
        }

        //private static string _thirdPartyAuthenticationRoutePath = "oauth2/thirdparty/token";

        //public static string ThirdPartyAuthenticationRoutePath
        //{
        //    get
        //    {
        //        return Configuration["ThirdPartyAuthenticationRoutePath"] ?? _thirdPartyAuthenticationRoutePath;
        //    }
        //    internal set
        //    {
        //        _thirdPartyAuthenticationRoutePath = value;
        //    }
        //}

        public static Register Register
        {
            get
            {
                var result = new Register();
                var section = Configuration.GetSection("Register");
                if (section != null)
                    result = section.Get<Register>();
                return result;
            }
        }

        public static ServicePart ServicePart
        {
            get
            {
                var result = new ServicePart();
                var section = Configuration.GetSection("ServicePart");
                if (section != null)
                    result = section.Get<ServicePart>();
                return result;
            }
        }

        public static AccessPolicy Policy
        {
            get
            {
                var result = new AccessPolicy();
                var section = Configuration.GetSection("AccessPolicy");
                if (section != null)
                    result = section.Get<AccessPolicy>();
                return result;
            }
        }

        public static JwtConfig JwtConfig
        {
            get
            {
                var result = new JwtConfig();
                var section = Configuration.GetSection("JwtConfig");
                if (section != null)
                    result = section.Get<JwtConfig>();
                return result;
            }
        }

        public static IEnumerable<string> WhiteList
        {
            get
            {
                IEnumerable<string> whiteList = new List<string>();
                if (Configuration.GetSection("WhiteList").Exists())
                {
                    whiteList = Configuration.GetSection("WhiteList").Get<ICollection<string>>();
                }

                return whiteList;
            }
        }

        private static string _cacheMode = "MemoryCache";

        public static string CacheMode
        {
            get
            {
                return Configuration["CacheMode"] ?? _cacheMode;
            }
        }
    }
}
