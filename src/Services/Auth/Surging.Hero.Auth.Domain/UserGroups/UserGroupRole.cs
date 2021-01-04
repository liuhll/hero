using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupRole : AuditedEntity<long>, IMultiTenant
    {
        public long RoleId { get; set; }

        public long UserGroupId { get; set; }
        
        public long? TenantId { get; set; }
    }
}