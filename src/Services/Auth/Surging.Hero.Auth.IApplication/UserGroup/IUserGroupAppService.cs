using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.Trees;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.UserGroup
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IUserGroupAppService : IServiceKey
    {
        [HttpPost(true)]
        [ServiceRoute("create")]
        Task<string> Create(CreateUserGroupInput input);

        [HttpPut(true)]
        [ServiceRoute("update")]
        Task<string> Update(UpdateUserGroupInput input);

        [ServiceRoute("get/{id}")]
        [HttpGet(true)]
        Task<GetUserGroupOutput> Get(long id);

        [ServiceRoute("delete/{id}")]
        [HttpDelete(true)]
        Task<string> Delete(long id);

        [HttpGet(true)]
        [ServiceRoute("get/tree")]
        Task<IEnumerable<ITree<GetUserGroupTreeOutput>>> GetTree();
    }
}
