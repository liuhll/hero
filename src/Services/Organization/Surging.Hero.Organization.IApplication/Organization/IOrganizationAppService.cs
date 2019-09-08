using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Organization.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Organization.IApplication.Organization
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IOrganizationAppService : IServiceKey
    {
        [HttpGet(true)]
        Task<IEnumerable<GetOrganizationTreeOutput>> GetTree();

        [HttpPost(true)]
        Task<IPagedResult<QueryOrganizationOutput>> Query(QueryOrganizationInput query);
    }
}
