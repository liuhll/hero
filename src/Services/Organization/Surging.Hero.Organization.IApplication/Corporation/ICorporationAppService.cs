using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Common;
using Surging.Hero.Common.Dtos;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;
using System.Threading.Tasks;

namespace Surging.Hero.Organization.IApplication.Corporation
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface ICorporationAppService : IServiceKey
    {
        Task<string> Create(CreateCorporationInput input);

        Task<string> Update(UpdateCorporationInput input);

        Task<string> Delete(DeleteByIdInput input);

        [ServiceRoute("{id}")]
        Task<GetCorporationOutput> Get(long id);
    }
}
