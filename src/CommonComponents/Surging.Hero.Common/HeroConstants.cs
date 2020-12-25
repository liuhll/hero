using Surging.Cloud.ProxyGenerator.Interceptors.Implementation.Metadatas;

namespace Surging.Hero.Common
{
    public static class HeroConstants
    {
        public const string RouteTemplet = "api/{appService}";

        public const int UnDeletedFlag = 0;

        public const int DeletedFlag = 1;

        public static string CacheProviderKey = "ddlCache.Redis"; 

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
        }
    }
}