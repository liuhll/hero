using Surging.Hero.Organization.Domain.Shared.Organizations;

namespace Surging.Hero.Organization.Domain
{
    public class Organization
    {
        public long Id { get; set; }

        public long ParentId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Memo { get; set; }

        public OrganizationType OrganizationType { get; set; }


    }
}
