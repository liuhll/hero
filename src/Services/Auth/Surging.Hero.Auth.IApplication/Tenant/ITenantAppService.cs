using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Hero.Auth.IApplication.Tenant.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Tenant
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface ITenantAppService : IServiceKey
    {
        [ServiceRoute("")]
        [HttpPost]
        [Service(Name = "新增租户", Director = Developers.Liuhll, Date = "2021-01-04")]
        Task<string> Create(CreateTenantInput input);
        
        [ServiceRoute("")]
        [HttpPut]
        [Service(Name = "新增租户", Director = Developers.Liuhll, Date = "2021-01-04")]
        Task<string> Update(UpdateTenantInput input);

        [ServiceRoute("")]
        [HttpPut]
        [Service(Name = "删除租户", Director = Developers.Liuhll, Date = "2021-01-05")]
        Task<string> Delete(long id);

        [ServiceRoute("search")]
        [HttpPut]
        [Service(Name = "搜索租户", Director = Developers.Liuhll, Date = "2021-01-05")]
        Task<IPagedResult<GetTenantOutput>> Search(QueryTenantInput query);
        
        [HttpPut]
        [Service( Name = "激活/冻结租户", Director = Developers.Liuhll, Date = "2021-01-05")]
        [ServiceRoute("status")]
        Task<string> Status(UpdateTenantStatusInput input);
    }
}