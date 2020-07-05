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
        /// <summary>
        /// 获取职位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceRoute("get/{id}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetPositionById, Mode = Core.Caching.CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取职位信息", AllowPermission = true)]
        Task<GetPositionOutput> Get([CacheKey(1)]long id);

        /// <summary>
        /// 根据部门id获取部门职位
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [ServiceRoute("get/dept/{deptId}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptPositionById, Mode = Core.Caching.CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据部门id获取部门职位", AllowPermission = true)]
        Task<IEnumerable<GetPositionOutput>> GetDeptPosition([CacheKey(1)]long deptId);

        /// <summary>
        /// 根据组织机构id获取职位列表
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [ServiceRoute("get/org/{orgId}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptPositionByOrgId, Mode = Core.Caching.CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据组织机构id获取职位列表")]
        Task<IEnumerable<GetPositionOutput>> GetDeptPositionByOrgId([CacheKey(1)]long orgId);

        /// <summary>
        /// 检查职位是否允许删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ServiceRoute("check/delete")]
        [HttpPost]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "检查职位是否允许删除", DisableNetwork = true)]
        Task<bool> CheckCanDeletePosition(CheckCanDeletePositionInput input);

        /// <summary>
        /// 检查某个职位是否允许删除
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        [HttpPost(true)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "检查某个职位是否允许删除", DisableNetwork = true)]
        Task<bool> Check(long positionId);
    }
}
