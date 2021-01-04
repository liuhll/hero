using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.Roles
{
    public class RolePermission : AuditedEntity<long>, IMultiTenant
    {
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
        
        public long? TenantId { get; set; }
    }
}