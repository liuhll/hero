namespace Surging.Hero.Organization.IApplication
{
    public static class CacheKeyConstant
    {
        public const string GetDeptPositionByOrgId = "GetDeptPositionBy:OrgId:{0}";

        public const string GetDeptPositionById = "GetDeptPositionBy:Id:{0}";

        public const string RemoveGetDeptPositionKey = "GetDeptPositionBy:*";

        public const string GetPositionById = "GetPositionById:{0}";

        public const string RemoveGetPositionByIdKey = "GetPositionBy:*";

        public const string GetDeptByOrgId = "GetDeptBy:OrgId:{0}";

        public const string GetDeptById = "GetDeptBy:Id:{0}";

        public const string RemoveGetDeptKey = "GetDeptBy:*";

        public const string GetSubOrgIds = "GetSubOrgIds:{0}";

        public const string RemoveGetSubOrgIds = "GetSubOrgIds:*";

        public const string GetOrgById = "GetOrgById:{0}";

        public const string GetParentsOrgsById = "GetOrgById:Parents:{0}";
        
        public const string RemoveGetOrgId = "GetOrgById:*";
        
        public const string RemoveDeptPositionById = "GetDeptPositionBy:*";
    }
}