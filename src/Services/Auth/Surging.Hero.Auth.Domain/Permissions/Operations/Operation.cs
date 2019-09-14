using Surging.Core.Domain.Entities.Auditing;
using Surging.Hero.Auth.Domain.Shared.Operations;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Permissions.Operations
{
    public class Operation : FullAuditedEntity<long>
    {
        public string Name { get; set; }

        public OperationMold Mold { get; set; }

        public string Memo { get; set; }

        public Status Status { get; set; }
    }
}
