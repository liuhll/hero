using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Organization.Domain
{
    public class DeptPosition : AuditedEntity<long>
    {
        public long DeptId { get; set; }

        public long PositionId { get; set; }
    }
}
