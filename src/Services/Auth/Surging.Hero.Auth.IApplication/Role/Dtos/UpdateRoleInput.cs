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
        

        
    }
}