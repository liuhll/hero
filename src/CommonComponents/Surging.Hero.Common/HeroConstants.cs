using Surging.Cloud.ProxyGenerator.Interceptors.Implementation.Metadatas;

namespace Surging.Hero.Common
{
    public static class HeroConstants
    {
        public const string RouteTemplet = "api/{appService}";
        
        public const string CaptachaRouteTemplet = "api/{ImageAppService}";

        public const int UnDeletedFlag = 0;

        public const int DeletedFlag = 1;

        public const string CacheProviderKey = "ddlCache.Redis";
        
        public const string DefaultSysName = "Hero权限管理系统";

        public static class CodeRuleRestrain
        {
            public const string CodeSeparator = ".";

            public const char CodeCoverSymbol = '0';

            public const int CodeCoverBit = 4;

            public const string FullNameSeparator = "-";
        }
        
        public static class CacheKey
        {
            public const string PermissionCheck = "PermissionCheck:{0}:{1}";

            public const string RemoveUserPermissionCheck = "PermissionCheck:*:{0}";

            public const string Captcha = "Captcha:{0}";
        }
    }
}