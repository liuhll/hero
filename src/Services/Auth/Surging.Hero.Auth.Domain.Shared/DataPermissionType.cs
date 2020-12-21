using System.ComponentModel;

namespace Surging.Hero.Auth.Domain.Shared
{
    public enum DataPermissionType
    {
        /// <summary>
        /// 拥有所有组织机构的权限
        /// </summary>
        [Description("所有部门权限")]
        AllOrg = 9999,
        
        /// <summary>
        /// 只拥有本部门的权限
        /// </summary>
        [Description("本部门权限")]
        OnlySelfOrg = 1,
        
        /// <summary>
        /// 拥有本级部门和下级部门的权限
        /// </summary>
        [Description("本部门和下级部门权限")]
        SelfAndLowerOrg = 2,
        
        /// <summary>
        /// 用户自定义部门的权限
        /// </summary>
        [Description("用户自定义部门权限")]
        UserDefined = 3
    }
}