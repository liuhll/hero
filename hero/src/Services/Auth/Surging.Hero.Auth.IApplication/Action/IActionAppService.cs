using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Auth.IApplication.Action.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.Action
{
    [ServiceBundle("v1/api/action")]
    public interface IActionAppService : IServiceKey
    {
        [Service(DisableNetwork = true)]
        Task<string> InitActions(ICollection<InitActionActionInput> actions);
    }
}
