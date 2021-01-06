using System.Collections.Generic;
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
        /// <summary>
        /// 新增租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ServiceRoute("")]
        [HttpPost]
        [Service(Name = "新增租户", Director = Developers.Liuhll, Date = "2021-01-04")]
        Task<string> Create(CreateTenantInput input);
        
        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ServiceRoute("")]
        [HttpPut]
        [Service(Name = "更新租户", Director = Developers.Liuhll, Date = "2021-01-04")]
        Task<string> Update(UpdateTenantInput input);

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceRoute("{id}")]
        [HttpDelete]
        [Service(Name = "删除租户", Director = Developers.Liuhll, Date = "2021-01-05")]
        Task<string> Delete(long id);

        /// <summary>
        /// 搜索租户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [ServiceRoute("search")]
        [HttpPost]
        [Service(Name = "搜索租户", Director = Developers.Liuhll, Date = "2021-01-05")]
        Task<IPagedResult<GetTenantPageOutput>> Search(QueryTenantInput query);
        
        /// <summary>
        /// 获取有效的租户列表
        /// </summary>
        /// <returns></returns>
        [ServiceRoute("list")]
        [HttpPost]
        [Service(Name = "获取租户列表", EnableAuthorization = false, Director = Developers.Liuhll, Date = "2021-01-05")]
        Task<IEnumerable<GetTenantOutput>> List();        

        /// <summary>
        /// 激活/冻结租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Service( Name = "激活/冻结租户", Director = Developers.Liuhll, Date = "2021-01-05")]
        [ServiceRoute("status")]
        Task<string> Status(UpdateTenantStatusInput input);
    }
}