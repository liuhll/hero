using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Hero.Organization.Domain.Shared.Organizations;

namespace Surging.Hero.Organization.IApplication.Organization.Dtos
{
    public class QueryOrganizationInput : PagedResultRequestDto
    {
        public string Code { get; set; }

        public OrganizationType? OrganizationType { get; set; }

        public string SearchKey { get; set; }
    }
}