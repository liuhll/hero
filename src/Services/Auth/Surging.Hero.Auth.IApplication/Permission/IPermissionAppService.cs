using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using Surging.Hero.Common;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.Permission
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IPermissionAppService : IServiceKey
    {
        [HttpPost(true)]
        Task<string> CreateMenu(CreateMenuInput input);

        [HttpPut(true)]
        Task<string> Update(UpdateMenuInput input);

        [HttpGet(true)]
        [ServiceRoute("{id}")]
        Task<GetMenuOutput> GetMenu(long id);
    }
}
