using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.Users
{
    public class UserRole : AuditedEntity<long>, IMultiTenant
    {
        public long UserId { get; set; }

        public long RoleId { get; set; }
        
        public long? TenantId { get; set; }
    }
}