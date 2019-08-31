
using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.Permissions.Operations
{
    public class OperationActionRelation : AuditedEntity<long>
    {
        public long OperationId { get; set; }

        public long ActionId { get; set; }

        public string ServiceId { get; set; }
    }
}
