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
        [ServiceRoute("menu/create")]
        Task<string> CreateMenu(CreateMenuInput input);

        [HttpPut(true)]
        [ServiceRoute("menu/update")]
        Task<string> UpdateMenu(UpdateMenuInput input);

        [HttpGet(true)]
        [ServiceRoute("get/menu/{id}")]
        Task<GetMenuOutput> GetMenu(long id);

        [HttpPost(true)]
        [ServiceRoute("operation/create")]
        Task<string> CreateOperation(CreateOperationInput input);

        [HttpPut(true)]
        [ServiceRoute("operation/update")]
        Task<string> UpdateOperation(UpdateOperationInput input);

        [HttpGet(true)]
        [ServiceRoute("get/operation/{id}")]
        Task<GetOperationOutput> GetOperation(long id);

        [HttpGet(true)]
        [ServiceRoute("get/tree")]
        Task<IEnumerable<GetPermissionTreeOutput>> GetTree();

        [HttpDelete(true)]
        [ServiceRoute("delete")]
        Task<string> Delete(DeletePermissionInput input);

        [HttpPost(true)]
        [Service(DisableNetwork = true, Name = "接口鉴权",Director = Developers.Liuhll)]
        [ServiceRoute("check")]
        Task<bool> Check(string serviceId);
    }
}
