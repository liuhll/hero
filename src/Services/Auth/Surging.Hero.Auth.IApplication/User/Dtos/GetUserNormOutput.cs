using Surging.Hero.Auth.IApplication.Role.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class GetUserNormOutput : GetUserBasicOutput
    {
        public IEnumerable<GetDisplayRoleOutput> Roles { get; set; }

        public string DisplayRoles { get { return string.Join(",", Roles.Select(p => p.Name)); } }
    }
}
