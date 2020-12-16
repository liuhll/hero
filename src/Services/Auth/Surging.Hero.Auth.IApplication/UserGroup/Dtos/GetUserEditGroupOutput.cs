using System.Collections.Generic;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class GetUserEditGroupOutput : UserGroupDtoBase
    {
        public long Id { get; set; }

        public IEnumerable<long> RoleIds { get; set; }

        public IEnumerable<long> PermissionIds { get; set; }
    }
}