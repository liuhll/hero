﻿using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupDataPermissionOrgRelation : AuditedEntity<long>
    {
        public long UserGroupId { get; set; }

        public long OrgId { get; set; }
    }
}