using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.Role
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IRoleAppService : IServiceKey
    {
        [HttpPost(true)]
        Task<string> Create(CreateRoleInput input);

        [HttpPut(true)]
        Task<string> Update(UpdateRoleInput input);

        [HttpPut(true)]
        Task<string> Status(UpdateRoleStatusInput input);

        [HttpGet(true)]
        [ServiceRoute("{id}")]
        Task<GetRoleOutput> Get(long id);

        [HttpPost(true)]
        Task<IPagedResult<GetRoleOutput>> Query(QueryRoleInput query);

        [HttpGet(true)]
        Task<IEnumerable<GetRoleOutput>> List(string searchKey);

        [HttpGet(true)]
        [ServiceRoute("{id}")]
        Task<IEnumerable<GetRolePermissionTreeOutput>> GetRolePermissions(long id);

        [HttpPut(true)]
        Task<string> SetPermissions(SetRolePermissionInput input);

        [HttpDelete(true)]
        [ServiceRoute("{id}")]
        Task<string> Delete(long id);
    }
}
