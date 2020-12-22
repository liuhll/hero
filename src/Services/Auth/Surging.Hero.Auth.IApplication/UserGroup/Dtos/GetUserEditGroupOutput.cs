using System.Collections.Generic;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class GetUserEditGroupOutput : UserGroupDtoBase
    {
        public long Id { get; set; }

        public long[] RoleIds { get; set; }

        public long[] PermissionIds { get; set; }
        
        public long[] OrgIds { get; set; }
    }
}