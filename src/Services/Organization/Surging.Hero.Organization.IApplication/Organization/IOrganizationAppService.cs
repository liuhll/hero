using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Cloud.Domain.Trees;
using Surging.Cloud.ProxyGenerator.Interceptors.Implementation.Metadatas;
using Surging.Cloud.System.Intercept;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Organization.Dtos;

namespace Surging.Hero.Organization.IApplication.Organization
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IOrganizationAppService : IServiceKey
    {
        /// <summary>
        ///     获取组织结构树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ServiceRoute("tree")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取组织结构树形结构", AllowPermission = true)]
        Task<IEnumerable<ITree<GetOrganizationTreeOutput>>> GetTree();

        [HttpGet]
        [ServiceRoute("own/tree")]
        [Service(Director = Developers.Liuhll, Date = "2020-12-28", Name = "获取当前登录用户有数据权限的组织树")]
        Task<IEnumerable<ITree<GetOrganizationTreeOutput>>> GetOwnTree();
        

        /// <summary>
        ///     查询组织机构
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "查询组织机构", AllowPermission = true)]
        [ServiceRoute("search")]
        Task<IPagedResult<GetOrganizationOutput>> Search(QueryOrganizationInput query);

        /// <summary>
        ///     获取子机构Id
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [HttpGet]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetSubOrgIds, Mode = CacheTargetType.Redis)]
        [ServiceRoute("suborgs/{orgId}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取子机构Id", DisableNetwork = true)]
        Task<IEnumerable<long>> GetSubOrgIds([CacheKey(1)] long orgId);
        
        [HttpGet]
        [ServiceRoute("parents/{orgId}")]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetParentsOrgsById, Mode = CacheTargetType.Redis)]
        [Service(Director = Developers.Liuhll, Date = "2020-12-28", Name = "获取指定Org的父级组织机构", DisableNetwork = true)]
        Task<IEnumerable<GetOrganizationTreeOutput>> GetParentsOrgs([CacheKey(1)]long orgId);
        

        [HttpGet]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetOrgById, Mode = CacheTargetType.Redis)]
        [ServiceRoute("{orgId}")]
        [Service(Director = Developers.Liuhll, Date = "2020-12-22", Name = "根据组织id获取组织机信息")]
        Task<GetOrganizationOutput> GetOrg([CacheKey(1)]long orgId);
    }
}