using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Position.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Organization.IApplication.Position
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IPositionAppService : IServiceKey
    {
        [ServiceRoute("{id}")]
        [HttpGet(true)]
        Task<GetPositionOutput> Get(long id);

        [ServiceRoute("{deptId}")]
        [HttpGet(true)]
        Task<IEnumerable<GetPositionOutput>> GetDeptPosition(long deptId);

        [HttpPost(true)]
        Task<string> Create(CreatePositionInput input);

        [HttpPut(true)]
        Task<string> Update(UpdatePositionInput input);

        [ServiceRoute("{id}")]
        [HttpDelete(true)]
        Task<string> Delete(long id);

        [Service(DisableNetwork = true)]
        [HttpPost(true)]
        Task<bool> Check(long positionId);
    }
}
