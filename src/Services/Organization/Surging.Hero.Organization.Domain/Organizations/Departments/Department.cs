using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Organization.Domain
{
    public class Department : FullAuditedEntity<long>
    {

        public long OrgId { get; set; }

        public string Location { get; set; }

        public long DeptTypeId { get; set; }

        public string BriefIntro { get; set; }

    }
}
