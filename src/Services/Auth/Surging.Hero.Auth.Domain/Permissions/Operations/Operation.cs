using Surging.Cloud.Domain.Entities.Auditing;
using Surging.Hero.Auth.Domain.Shared.Operations;

namespace Surging.Hero.Auth.Domain.Permissions.Operations
{
    public class Operation : FullAuditedEntity<long>
    {
        public long PermissionId { get; set; }

        public long MenuId { get; set; }

        public string Code { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public OperationMold Mold { get; set; }

        public string Memo { get; set; }
    }
}