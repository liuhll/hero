using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class CreateRoleInput : RoleDtoBase
    {
        /// <summary>
        ///     选定的权限Ids
        /// </summary>
        public long[] PermissionIds { get; set; }

        /// <summary>
        ///  用户自定义数据权限指定的部门
        /// </summary>
        public long[] DataPermissionOrgIds { get; set; }
        
        /// <summary>
        /// 所属部门
        /// </summary>
        [Required(ErrorMessage = "所属部门不允许为空")]
        public long[] OrgIds { get; set; }

        /// <summary>
        ///  角色是否允许分配给所有部门
        /// </summary>
        public bool IsAllOrg { get; set; }
    }
}