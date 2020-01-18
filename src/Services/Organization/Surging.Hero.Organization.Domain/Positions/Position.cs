using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Organization.Domain.Positions
{
    public class Position : FullAuditedEntity<long>
    {
        public long DeptId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long FunctionId { get; set; }

        public long PositionLevelId { get; set; }

        public string PostResponsibility { get; set; }

        public bool IsLeadingOfficial { get; set; }

        public bool IsLeadershipPost { get; set; }
    }
}
