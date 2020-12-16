using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
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