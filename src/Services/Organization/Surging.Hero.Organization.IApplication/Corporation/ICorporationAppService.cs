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
        Task<string> Create(CreateCorporationInput input);

        [HttpPut(true)]
        Task<string> Update(UpdateCorporationInput input);

        [ServiceRoute("{orgId}")]
        [HttpDelete(true)]
        Task<string> DeleteByOrgId(long orgId);

        [ServiceRoute("{orgId}")]
        [HttpGet(true)]
        Task<GetCorporationOutput> GetByOrgId(long orgId);
    }
}
