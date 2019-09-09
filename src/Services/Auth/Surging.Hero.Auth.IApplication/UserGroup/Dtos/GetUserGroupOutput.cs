using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using System.Collections.Generic;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class GetUserGroupOutput : UserGroupDtoBase
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public IEnumerable<GetDisplayRoleOutput> Roles { get; set; }

        public IEnumerable<GetUserOutput> Users { get; set; }
    }
}
