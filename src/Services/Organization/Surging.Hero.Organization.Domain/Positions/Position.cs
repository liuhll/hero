using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.Organization.Domain.Positions
{
    public class Position : FullAuditedEntity<long>, IMultiTenant
    {
        public long DeptId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string FunctionKey { get; set; }

        public string PositionLevelKey { get; set; }

        public string PostResponsibility { get; set; }

        public bool IsLeadingOfficial { get; set; }

        public bool IsLeadershipPost { get; set; }
        
        public long? TenantId { get; set; }
    }
}