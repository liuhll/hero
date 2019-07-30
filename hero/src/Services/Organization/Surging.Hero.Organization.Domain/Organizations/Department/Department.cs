using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Organization.Domain
{
    public class Department : FullAuditedEntity<long>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public long ParentId { get; set; }

        public long CorporationId { get; set; }

        public string Location { get; set; }

        public string DeptTypeId { get; set; }

        public string BriefIntro { get; set; }

        public string Memo { get; set; }
    }
}
