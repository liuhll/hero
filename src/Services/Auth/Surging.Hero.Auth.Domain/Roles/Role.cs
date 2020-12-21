using Surging.Core.Domain.Entities.Auditing;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Roles
{
    public class Role : FullAuditedEntity<long>, IOrgAudited
    {
        public string Name { get; set; }

        public string Memo { get; set; }

        public Status Status { get; set; }
        
        public long? OrgId { get; set; }
    }
}