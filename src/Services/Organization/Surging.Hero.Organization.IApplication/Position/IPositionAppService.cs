using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.System.Intercept;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Position.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Organization.IApplication.Position
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IPositionAppService : IServiceKey
    {
        [ServiceRoute("get/{id}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetPositionById, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<GetPositionOutput> Get([CacheKey(1)]long id);

        [ServiceRoute("get/dept/{deptId}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptPositionById, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<IEnumerable<GetPositionOutput>> GetDeptPosition([CacheKey(1)]long deptId);


        [ServiceRoute("get/org/{orgId}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptPositionByOrgId, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<IEnumerable<GetPositionOutput>> GetDeptPositionByOrgId([CacheKey(1)]long orgId);

        [Service(DisableNetwork = true)]
        [HttpPost(true)]
        Task<bool> Check(long positionId);
    }
}
