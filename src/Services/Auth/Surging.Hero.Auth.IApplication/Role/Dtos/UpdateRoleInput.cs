using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class UpdateRoleInput : RoleDtoBase
    {
        /// <summary>
        ///     选定的角色Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     选定的权限Ids
        /// </summary>
        public long[] PermissionIds { get; set; }

        public long[] DataPermissionOrgIds { get; set; }
        
        /// <summary>
        /// 所属部门
        /// </summary>
        [Required(ErrorMessage = "所属部门不允许为空")]
        public long[] OrgIds { get; set; }

        /// <summary>
        /// 角色是否可以被分配给所有用户
        /// </summary>
        public bool IsAllOrg { get; set; }
    }
}