using System;
using System.Collections.Generic;
using System.Linq;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class GetUserGroupOutput : UserGroupDtoBase
    {
        public long Id { get; set; }

        public IEnumerable<GetDisplayRoleOutput> Roles { get; set; }

        public string DisplayRoles
        {
            get { return string.Join(",", Roles.Select(p => p.Name)); }
        }

        public IEnumerable<long> RoleIds
        {
            get { return Roles.Select(p => p.Id); }
        }

        public IEnumerable<GetDisplayPermissionOutput> Permissions { get; set; }

        public string DisplayPermissions
        {
            get { return string.Join(",", Permissions.Select(p => p.Title)); }
        }
        
        public IEnumerable<long> PermissionIds
        {
            get { return Permissions.Select(p => p.Id); }
        }


        public IEnumerable<GetDisplayDataPermissionOrgOutput> DataPermissionOrgs { get; set; }

        public IEnumerable<long> OrgIds
        {
            get { return DataPermissionOrgs.Select(p => p.Id); }
        }

        public string DisplayDataPermissionOrgs
        {
            get { return string.Join(",", DataPermissionOrgs.Select(p => p.Name)); }
        }

        /// <summary>
        ///     用户组状态
        /// </summary>
        public Status Status { get; set; }

        public virtual DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }

        public virtual string CreatorUserName { get; set; }

        /// <summary>
        ///     最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        public string LastModificationUserName { get; set; }

        /// <summary>
        ///     最后修改人
        /// </summary>
        public long? LastModifierUserId { get; set; }
    }
}