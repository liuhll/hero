using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;
using System.Threading.Tasks;

namespace Surging.Hero.Organization.IApplication.Corporation
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface ICorporationAppService : IServiceKey
    {
        [HttpPost(true)]
        [ServiceRoute("create")]
        Task<string> Create(CreateCorporationInput input);

        [HttpPut(true)]
        [ServiceRoute("update")]
        Task<string> Update(UpdateCorporationInput input);

        [ServiceRoute("delete/{orgId}")]
        [HttpDelete(true)]
        Task<string> DeleteByOrgId(long orgId);

        [ServiceRoute("get/org/{orgId}")]
        [HttpGet(true)]
        Task<GetCorporationOutput> GetByOrgId(long orgId);
    }
}
