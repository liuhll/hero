using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
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

        public IEnumerable<GetDisplayUserGroupOutput> UserGroups { get; set; }

        public IEnumerable<long> UserGroupIds
        {
            get
            {
                return UserGroups.Select(p => p.Id);
            }
        }

        public string DisplayUserGroups { get { return string.Join(",", UserGroups.Select(p => p.Name)); } }

        public virtual DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }

        public virtual string CreatorUserName { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public string LastModificationUserName { get; set; }

        public long? LastModifierUserId { get; set; }


        public long DeptId { get; set; }
    }
}
