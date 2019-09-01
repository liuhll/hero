using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Common.Dtos;
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

        [HttpDelete(true)]
        Task<string> Delete(DeleteByIdInput input);

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

        [HttpDelete(true)]
        Task<string> DeleteWordbookItem(DeleteByIdInput input);

        [ServiceRoute("{id}")]
        [HttpGet(true)]
        Task<GetWordbookItemOutput> GetWordbookItem(long id);

        [HttpPost(true)]
        Task<bool> Check(CheckWordbookInput input);
    }
}
