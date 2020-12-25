using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.Roles
{
    public class RoleDataPermissionOrgRelation : AuditedEntity<long>
    {
        public long RoleId { get; set; }

        public long OrgId { get; set; }
    }
}