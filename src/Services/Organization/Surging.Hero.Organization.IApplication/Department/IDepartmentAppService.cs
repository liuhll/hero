using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.System.Intercept;
using Surging.Hero.Common;
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

        [ServiceRoute("{orgId}")]
        [HttpDelete(true)]
        Task<string> DeleteByOrgId(long orgId);

        [ServiceRoute("{id}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptById, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<GetDepartmentOutput> Get(long id);

        [ServiceRoute("{orgId}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptByOrgId, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<GetDepartmentOutput> GetByOrgId(long orgId);

        [Service(DisableNetwork = true)]
        [HttpPost(true)]
        Task<bool> Check(long orgId);
    }
}
