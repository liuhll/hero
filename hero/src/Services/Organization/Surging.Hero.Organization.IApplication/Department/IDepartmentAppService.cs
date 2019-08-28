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
        Task<string> Create(CreateDepartmentInput input);

        Task<string> Update(UpdateDepartmentInput input);

        Task<string> Delete(DeleteByIdInput input);

        [ServiceRoute("{id}")]
        Task<GetDepartmentOutput> Get(long id);

        [Service(DisableNetwork = true)]
        Task<bool> Check(long deptId);
    }
}
