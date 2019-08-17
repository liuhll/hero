using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Organization.Domain.Shared.Organizations;

namespace Surging.Hero.Organization.IApplication.Organization.Dtos
{
    public class QueryOrganizationInput :  PagedResultRequestDto
    {
        public long? Id { get; set; }

        public OrganizationType OrganizationType { get; set; }

        public string SearchKey { get; set; }
    }
}
