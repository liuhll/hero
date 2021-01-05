using Surging.Cloud.Domain.PagedAndSorted;

namespace Surging.Hero.Auth.IApplication.Tenant.Dtos
{
    public class QueryTenantInput : PagedAndSingleSortedResultRequest
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        public string Name { get; set; }
    }
}