using System.Collections;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class CreateUserGroupInput : UserGroupDtoBase
    {
        public long[] RoleIds { get; set; }

        public long[] PermissionIds { get; set; }

        public long[] DataPermissionOrgIds { get; set; }
        
    }
}