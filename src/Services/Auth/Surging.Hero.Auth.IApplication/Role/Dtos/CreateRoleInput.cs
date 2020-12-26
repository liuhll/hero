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
        
    }
}