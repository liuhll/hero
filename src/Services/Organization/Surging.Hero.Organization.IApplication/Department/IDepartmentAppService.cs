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
        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost(true)]
        [ServiceRoute("create")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增部门信息")]
        Task<CreateDepartmentOutput> Create(CreateDepartmentInput input);

        /// <summary>
        /// 更新部门信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ServiceRoute("update")]
        [HttpPut(true)]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new string[] { CacheKeyConstant.RemoveGetDeptKey, CacheKeyConstant.RemoveGetSubOrgIds }, Mode = Core.Caching.CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "更新部门信息")]
        Task<UpdateDepartmentOutput> Update(UpdateDepartmentInput input);

        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [ServiceRoute("delete/{orgId}")]
        [HttpDelete(true)]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new string[] { CacheKeyConstant.RemoveGetDeptKey, CacheKeyConstant.RemoveGetSubOrgIds }, Mode = Core.Caching.CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "删除部门信息")]
        Task<string> DeleteByOrgId(long orgId);

        /// <summary>
        /// 根据部门id获取部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceRoute("get/{id}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptById, Mode = Core.Caching.CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据部门id获取部门信息", AllowPermission = true)]
        Task<GetDepartmentOutput> Get([CacheKey(1)]long id);

        /// <summary>
        /// 根据组织id获取部门信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [ServiceRoute("get/orgid/{orgId}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptByOrgId, Mode = Core.Caching.CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据组织id获取部门信息")]
        Task<GetDepartmentOutput> GetByOrgId([CacheKey(1)]long orgId);

        /// <summary>
        /// 检查一个部门是否存在
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [HttpPost(true)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "检查一个部门是否存在", DisableNetwork = true)]
        Task<bool> Check(long orgId);
    }
}
