using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;

namespace Surging.Hero.Organization.IApplication.Corporation
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface ICorporationAppService : IServiceKey
    {
        /// <summary>
        ///     新增公司信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceRoute("")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增公司信息")]
        Task<CreateCorporationOutput> Create(CreateCorporationInput input);
        
        
        [Service(Director = Developers.Liuhll, DisableNetwork = true,Date = "2021-01-05", Name = "新增租户时,默认增加对应的组织机构")]
        Task<CreateCorporationOutput> CreateByTenant(CreateCorporationByTenantInput input);

        /// <summary>
        ///     更新公司信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [ServiceRoute("")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "更新公司信息")]
        Task<UpdateCorporationOutput> Update(UpdateCorporationInput input);

        /// <summary>
        ///     删除公司
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [ServiceRoute("{orgId}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "删除公司")]
        [HttpDelete]
        Task<string> DeleteByOrgId(long orgId);

        /// <summary>
        ///     获取公司信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [ServiceRoute("org/{orgId}")]
        [HttpGet]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取公司信息", AllowPermission = true)]
        Task<GetCorporationOutput> GetByOrgId(long orgId);
    }
}