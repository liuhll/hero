using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class GetUserNormOutput : GetUserBasicOutput
    {
        public IEnumerable<GetDisplayRoleOutput> Roles { get; set; }

        public IEnumerable<long> RoleIds { get {
                return Roles.Select(p => p.Id);    
            }
        }

        public string DisplayRoles { get { return string.Join(",", Roles.Select(p => p.Name)); } }

        public DateTime? LastModificationTime { get; set; }

        public string LastModificationUserName { get; set; }

        public long? LastModifierUserId { get; set; }
        public long DeptId { get; set; }
    }
}
