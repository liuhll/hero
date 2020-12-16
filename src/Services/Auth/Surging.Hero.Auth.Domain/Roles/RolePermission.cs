using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.Roles
{
    public class RolePermission : AuditedEntity<long>
    {
        public long RoleId { get; set; }

        public long PermissionId { get; set; }
    }
}