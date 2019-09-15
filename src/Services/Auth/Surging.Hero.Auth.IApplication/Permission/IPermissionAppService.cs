using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.Permission
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IPermissionAppService : IServiceKey
    {
        [HttpPost(true)]
        Task<string> CreateMenu(CreateMenuInput input);

        [HttpPut(true)]
        Task<string> UpdateMenu(UpdateMenuInput input);

        [HttpGet(true)]
        [ServiceRoute("{id}")]
        Task<GetMenuOutput> GetMenu(long id);

        [HttpPost(true)]
        Task<string> CreateOperation(CreateOperationInput input);

        [HttpPut(true)]
        Task<string> UpdateOperation(UpdateOperationInput input);

        [HttpGet(true)]
        [ServiceRoute("{id}")]
        Task<GetOperationOutput> GetOperation(long id);

        [HttpGet(true)]
        Task<IEnumerable<GetPermissionTreeOutput>> GetTree();
    }
}
