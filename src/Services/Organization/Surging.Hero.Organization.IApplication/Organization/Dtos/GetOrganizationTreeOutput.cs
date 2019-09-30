using Surging.Core.Domain;
using Surging.Hero.Organization.Domain.Shared.Organizations;
using System.Collections.Generic;

namespace Surging.Hero.Organization.IApplication.Organization.Dtos
{
    public class GetOrganizationTreeOutput : ITree<GetOrganizationTreeOutput>
    {

        public long Id { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Title { get { return Name; } }

        public bool Expand { get { return true; } }

        public long ParentId { get; set; }
        public string FullName { get; set; }
        public IEnumerable<ITree<GetOrganizationTreeOutput>> Children { get; set; }
    }
}
