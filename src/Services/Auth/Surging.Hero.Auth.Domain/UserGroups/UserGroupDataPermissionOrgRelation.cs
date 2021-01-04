using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupDataPermissionOrgRelation : AuditedEntity<long>, IMultiTenant
    {
        public long UserGroupId { get; set; }

        public long OrgId { get; set; }
        
        public long? TenantId { get; set; }
    }
}