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
        Task<string> Create(CreateWordbookInput input);

        Task<string> Update(UpdateWordbookInput input);

        Task<string> Delete(DeleteByIdInput input);

        Task<IPagedResult<GetWordbookOutput>> Query(QueryWordbookInput query);

        [ServiceRoute("{id}")]
        Task<GetWordbookOutput> Get(long id);

        [ServiceRoute("{wordbookId}")]
        Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItems(long wordbookId);

        Task<string> CreateWordbookItem(CreateWordbookItemInput input);

        Task<string> UpdateWordbookItem(UpdateWordbookItemInput input);

        Task<string> DeleteWordbookItem(DeleteByIdInput input);

        [ServiceRoute("{id}")]
        Task<GetWordbookItemOutput> GetWordbookItem(long id);

        Task<bool> Check(CheckWordbookInput input);
    }
}
