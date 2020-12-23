namespace Surging.Hero.Auth.Application
{
    public static class AuthConstant
    {
        public static string[] UserGroupSortingFileds = {"Id", "UserName", "ChineseName", "UpdateTime", "CreateTime"};

        public static string[] UserSortingFileds = {"Id", "UserName", "ChineseName", "UpdateTime", "CreateTime"};

        public static class V1
        {
            public const string Version = "v1";

            public const string PermissionMoudleName = "permission.v1";

            public const string AccountMoudleName = "account.v1";
        }
        
    }
}