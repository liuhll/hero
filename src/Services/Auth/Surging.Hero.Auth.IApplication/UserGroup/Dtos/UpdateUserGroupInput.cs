using System.Collections.Generic;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class UpdateUserGroupInput : UserGroupDtoBase
    {
        public long Id { get; set; }

        public long[] RoleIds { get; set; }

        public long[] PermissionIds { get; set; }
        
        public long[] DataPermissionOrgIds { get; set; }
        
    }
}