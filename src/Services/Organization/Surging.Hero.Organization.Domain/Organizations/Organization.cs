using Surging.Core.Domain.Entities.Auditing;
using Surging.Hero.Organization.Domain.Shared.Organizations;

namespace Surging.Hero.Organization.Domain
{
    public class Organization : FullAuditedEntity<long>
    {
        public long ParentId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Identification { get; set; }

        public int Level { get; set; }

        public OrganizationType OrgType { get; set; }


    }
}
