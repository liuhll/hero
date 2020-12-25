using Surging.Cloud.Domain.Entities.Auditing;
using Surging.Hero.Auth.Domain.Shared.Permissions;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Permissions
{
    public class Permission : FullAuditedEntity<long>
    {
        public string Name { get; set; }

        public PermissionMold Mold { get; set; }

        public string Memo { get; set; }

        public Status Status { get; set; }
    }
}