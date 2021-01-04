using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserUserGroupRelation : AuditedEntity<long>, IMultiTenant
    {
        public long UserId { get; set; }

        public long UserGroupId { get; set; }
        
        public long? TenantId { get; set; }
    }
}