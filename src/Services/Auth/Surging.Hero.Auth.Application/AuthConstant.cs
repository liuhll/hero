namespace Surging.Hero.Auth.Application
{
    public static class AuthConstant
    {
        public static class V1
        {

            public const string Version = "v1";

            public const string PermissionMoudleName = "permission.v1";

            public const string AccountMoudleName = "account.v1";
        }

        public static string[] UserGroupSortingFileds = new string[] { "Id", "UserName", "ChineseName", "UpdateTime", "CreateTime" };

        public static string[] UserSortingFileds = new string[] { "Id", "UserName", "ChineseName", "UpdateTime", "CreateTime" };

        
    }
}
