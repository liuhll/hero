using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.BasicData.IApplication.Wordbook
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IWordbookAppService : IServiceKey
    {
        [HttpPost(true)]
        Task<string> Create(CreateWordbookInput input);

        [HttpPut(true)]
        Task<string> Update(UpdateWordbookInput input);

        [ServiceRoute("{id}")]
        [HttpDelete(true)]
        Task<string> Delete(long id);

        Task<IPagedResult<GetWordbookOutput>> Query(QueryWordbookInput query);

        [ServiceRoute("{id}")]
        [HttpGet(true)]
        Task<GetWordbookOutput> Get(long id);

        [ServiceRoute("{wordbookId}")]
        [HttpGet(true)]
        Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItems(long wordbookId);

        [HttpPost(true)]
        Task<string> CreateWordbookItem(CreateWordbookItemInput input);

        [HttpPut(true)]
        Task<string> UpdateWordbookItem(UpdateWordbookItemInput input);

        [ServiceRoute("{id}")]
        [HttpDelete(true)]
        Task<string> DeleteWordbookItem(long id);

        [ServiceRoute("{id}")]
        [HttpGet(true)]
        Task<GetWordbookItemOutput> GetWordbookItem(long id);

        [HttpPost(true)]
        Task<bool> Check(CheckWordbookInput input);
    }
}
