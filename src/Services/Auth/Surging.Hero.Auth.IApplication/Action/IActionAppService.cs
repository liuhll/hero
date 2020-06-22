using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Auth.IApplication.Action.Dtos;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.Action
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IActionAppService : IServiceKey
    {
        [Service(DisableNetwork = true)]
        [HttpPost(true)]
        [ServiceRoute("init")]
        Task<string> InitActions(ICollection<InitActionActionInput> actions);

        [ServiceRoute("get/host")]
        Task<IEnumerable<GetServiceHostOutput>> GetServiceHosts(QueryServiceHostInput query);

        [ServiceRoute("get/appservice")]
        Task<IEnumerable<GetAppServiceOutput>> GetAppServices(QueryAppServiceInput query);

        [ServiceRoute("get/service")]
        Task<IEnumerable<GetActionOutput>> GetServices(QueryActionInput query);
    }
}
