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
        [ServiceRoute("create")]
        Task<CreateDepartmentOutput> Create(CreateDepartmentInput input);

        [ServiceRoute("update")]
        [HttpPost(true)]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new string[] { CacheKeyConstant.RemoveGetDeptKey, CacheKeyConstant.RemoveGetSubOrgIds }, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<UpdateDepartmentOutput> Update(UpdateDepartmentInput input);

        [ServiceRoute("delete/{orgId}")]
        [HttpDelete(true)]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new string[] { CacheKeyConstant.RemoveGetDeptKey, CacheKeyConstant.RemoveGetSubOrgIds }, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<string> DeleteByOrgId(long orgId);

        [ServiceRoute("get/{id}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptById, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<GetDepartmentOutput> Get([CacheKey(1)]long id);

        [ServiceRoute("get/orgid/{orgId}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptByOrgId, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<GetDepartmentOutput> GetByOrgId([CacheKey(1)]long orgId);

        [Service(DisableNetwork = true)]
        [HttpPost(true)]
        Task<bool> Check(long orgId);
    }
}
