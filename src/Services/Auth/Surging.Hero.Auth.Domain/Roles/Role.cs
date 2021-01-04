using Surging.Cloud.Domain.Entities.Auditing;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Roles
{
    public class Role : FullAuditedEntity<long>, IMultiTenant
    {
        public string Name { get; set; }

        public string Identification { get; set; }

        public string Memo { get; set; }

        public bool IsAllOrg { get; set; }

        public Status Status { get; set; }
        
        public DataPermissionType DataPermissionType { get; set; }
        
        public long? TenantId { get; set; }
    }
}