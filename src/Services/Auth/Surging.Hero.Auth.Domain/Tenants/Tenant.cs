using Surging.Cloud.Domain.Entities.Auditing;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Tenants
{
    public class Tenant : FullAuditedEntity<long>
    {
        public string Name { get; set; }

        public string Memo { get; set; }

        public Status Status { get; set; }
        
    }
}