namespace Surging.Hero.Organization.IApplication
{
    public static class CacheKeyConstant
    {
        public const string GetDeptPositionByOrgId = "GetDeptPositionBy_OrgId_{0}";

        public const string GetDeptPositionById = "GetDeptPositionBy_Id_{0}";

        public const string RemoveGetDeptPositionKey = "GetDeptPositionBy_*";

        public const string GetPositionById = "GetPositionById_{0}";

        public const string RemoveGetPositionByIdKey = "GetPositionBy_*";

        public const string GetDeptByOrgId = "GetDeptBy_OrgId_{0}";

        public const string GetDeptById = "GetDeptBy_Id_{0}";

        public const string RemoveGetDeptKey = "GetDeptBy_*";

        public const string GetSubOrgIds = "GetSubOrgIds_{0}";

        public const string RemoveGetSubOrgIds = "GetSubOrgIds_*";

        public const string GetOrgById = "GetOrgById_{0}";
        
        public const string RemoveGetOrgId = "GetOrgById_*";
        
        public const string RemoveDeptPositionById = "GetDeptPositionBy_*";
    }
}