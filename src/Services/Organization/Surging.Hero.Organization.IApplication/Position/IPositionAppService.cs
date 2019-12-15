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
        [ServiceRoute("{id}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetPositionById, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<GetPositionOutput> Get([CacheKey(1)]long id);

        [ServiceRoute("{deptId}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptPositionById, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<IEnumerable<GetPositionOutput>> GetDeptPosition([CacheKey(1)]long deptId);


        [ServiceRoute("{orgId}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptPositionByOrgId, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<IEnumerable<GetPositionOutput>> GetDeptPositionByOrgId([CacheKey(1)]long orgId);

        [HttpPost(true)]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new[] { CacheKeyConstant.RemoveGetDeptPositionKey, CacheKeyConstant.RemoveGetPositionByIdKey, CacheKeyConstant.RemoveGetSubOrgIds }, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<string> Create(CreatePositionInput input);

        [HttpPut(true)]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new[] { CacheKeyConstant.RemoveGetDeptPositionKey, CacheKeyConstant.RemoveGetPositionByIdKey,CacheKeyConstant.RemoveGetDeptKey, CacheKeyConstant.RemoveGetSubOrgIds }, Mode = Core.Caching.CacheTargetType.Redis, CacheSectionType = SectionType.ddlCache)]
        Task<string> Update(UpdatePositionInput input);

        [ServiceRoute("{id}")]
        [HttpDelete(true)]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new[] { CacheKeyConstant.RemoveGetDeptPositionKey, CacheKeyConstant.RemoveGetPositionByIdKey, CacheKeyConstant.RemoveGetSubOrgIds }, Mode = Core.Caching.CacheTargetType.Redis, CacheSectionType = SectionType.ddlCache)]
        Task<string> Delete([CacheKey(1)]long id);

        [Service(DisableNetwork = true)]
        [HttpPost(true)]
        Task<bool> Check(long positionId);
    }
}
