using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.Domain.Trees;
using Surging.Core.ProxyGenerator.Interceptors.Implementation.Metadatas;
using Surging.Core.System.Intercept;
using Surging.Hero.Common;
using Surging.Hero.Organization.Domain.Shared.Organizations;
using Surging.Hero.Organization.IApplication.Organization.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Organization.IApplication.Organization
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IOrganizationAppService : IServiceKey
    {
        /// <summary>
        /// 获取组织结构树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ServiceRoute("tree")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取组织结构树形结构", AllowPermission = true)]
        Task<IEnumerable<ITree<GetOrganizationTreeOutput>>> GetTree();

        /// <summary>
        /// 查询组织机构
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "查询组织机构", AllowPermission = true)]
        [ServiceRoute("search")]
        Task<IPagedResult<QueryOrganizationOutput>> Query(QueryOrganizationInput query);

        /// <summary>
        /// 获取子机构Id
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        [HttpGet]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetSubOrgIds, Mode = CacheTargetType.Redis)]
        [ServiceRoute("suborgs/{orgId}")]
        [Service(Director = Developers.Liuhll, Date = "2020-07-04", Name = "获取子机构Id", DisableNetwork =true)]
        Task<IEnumerable<long>> GetSubOrgIds([CacheKey(1)]long orgId);

    }
}
