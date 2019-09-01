using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Common;
using Surging.Hero.Common.Dtos;
using Surging.Hero.Organization.IApplication.Department.Dtos;
using System.Threading.Tasks;

namespace Surging.Hero.Organization.IApplication.Department
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IDepartmentAppService : IServiceKey
    {
        [HttpPost(true)]
        Task<string> Create(CreateDepartmentInput input);

        [HttpPost(true)]
        Task<string> Update(UpdateDepartmentInput input);

        [HttpDelete(true)]
        Task<string> Delete(DeleteByIdInput input);

        [ServiceRoute("{id}")]
        [HttpGet(true)]
        Task<GetDepartmentOutput> Get(long id);

        [Service(DisableNetwork = true)]
        [HttpPost(true)]
        Task<bool> Check(long deptId);
    }
}
