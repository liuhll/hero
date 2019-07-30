using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Organization.Domain.Positions
{
    public class Position : AuditedEntity<long>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string ParentId { get; set; }

        public long FunctionId { get; set; }

        public long PositionTypeId { get; set; }

        public string BriefIntro { get; set; }

        public string Memo { get; set; }
    }
}
