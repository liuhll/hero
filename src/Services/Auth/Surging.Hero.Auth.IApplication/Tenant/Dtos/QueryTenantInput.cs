using Surging.Cloud.Domain.PagedAndSorted;

namespace Surging.Hero.Auth.IApplication.Tenant.Dtos
{
    public class QueryTenantInput : PagedAndSingleSortedResultRequest
    {
        public string Name { get; set; }
    }
}