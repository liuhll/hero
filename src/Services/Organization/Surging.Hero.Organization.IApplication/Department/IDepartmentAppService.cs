using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Cloud.ProxyGenerator.Interceptors.Implementation.Metadatas;
using Surging.Cloud.System.Intercept;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Department.Dtos;

namespace Surging.Hero.Organization.IApplication.Department
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IDepartmentAppService : IServiceKey
    {
        /// <summary>
        ///     获取公司信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceRoute("")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "新增部门信息")]
        Task<CreateDepartmentOutput> Create(CreateDepartmentInput input);

        /// <summary>
        ///     更新部门信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ServiceRoute("")]
        [HttpPut]
        [InterceptMethod(CachingMethod.Remove,
            CorrespondingKeys = new[] {CacheKeyConstant.RemoveGetDeptKey, CacheKeyConstant.RemoveGetSubOrgIds, CacheKeyConstant.RemoveGetOrgId, CacheKeyConstant.RemoveDeptPositionById},
            Mode = CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "更新部门信息")]
        Task<UpdateDepartmentOutput> Update(UpdateDepartmentInput input);

        /// <summary>
        ///     删除部门信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [ServiceRoute("{orgId}")]
        [HttpDelete]
        [InterceptMethod(CachingMethod.Remove,
            CorrespondingKeys = new[] {CacheKeyConstant.RemoveGetDeptKey, CacheKeyConstant.RemoveGetSubOrgIds, CacheKeyConstant.RemoveGetOrgId, CacheKeyConstant.RemoveDeptPositionById},
            Mode = CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "删除部门信息")]
        Task<string> DeleteByOrgId(long orgId);

        /// <summary>
        ///     根据部门id获取部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ServiceRoute("{id}")]
        [HttpGet]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptById, Mode = CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据部门id获取部门信息", AllowPermission = true)]
        Task<GetDepartmentOutput> Get([CacheKey(1)] long id);

        /// <summary>
        ///     根据组织id获取部门信息
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [ServiceRoute("org/{orgId}")]
        [HttpGet]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetDeptByOrgId, Mode = CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "根据组织id获取部门信息", AllowPermission = true)]
        Task<GetDepartmentOutput> GetByOrgId([CacheKey(1)] long orgId);

        /// <summary>
        ///     检查一个部门是否存在
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [HttpPost]
        [ServiceRoute("check/{orgId}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "检查一个部门是否存在", DisableNetwork = true)]
        Task<bool> Check(long orgId);
    }
}