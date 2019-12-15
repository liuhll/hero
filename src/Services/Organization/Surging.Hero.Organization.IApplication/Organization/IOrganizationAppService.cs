using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.Domain.Trees;
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
        [HttpGet(true)]
        [ServiceRoute("get/tree")]
        Task<IEnumerable<ITree<GetOrganizationTreeOutput>>> GetTree();

        [HttpPost(true)]
        Task<IPagedResult<QueryOrganizationOutput>> Query(QueryOrganizationInput query);

        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetSubOrgIds, Mode = Core.Caching.CacheTargetType.Redis)]
        [ServiceRoute("get/suborgs/{orgId}")]
        Task<IEnumerable<long>> GetSubOrgIds([CacheKey(1)]long orgId);

    }
}
