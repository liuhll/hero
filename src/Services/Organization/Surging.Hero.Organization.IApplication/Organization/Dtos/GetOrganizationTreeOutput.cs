using Surging.Hero.Organization.Domain.Shared.Organizations;
using System.Collections.Generic;

namespace Surging.Hero.Organization.IApplication.Organization.Dtos
{
    public class GetOrganizationTreeOutput
    {
        public GetOrganizationTreeOutput() {
            Children = new List<GetOrganizationTreeOutput>();
        }

        public long Id { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public IEnumerable<GetOrganizationTreeOutput> Children { get; set; }
    }
}
