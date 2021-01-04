using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.Organization.Domain
{
    public class Department : FullAuditedEntity<long>, IMultiTenant
    {
        public long OrgId { get; set; }

        public string Location { get; set; }

        public string DeptTypeKey { get; set; }

        public string BriefIntro { get; set; }
        
        public long? TenantId { get; set; }
    }
}