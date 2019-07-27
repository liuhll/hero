using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;
using Surging.Hero.Common.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.BasicData.IApplication.Wordbook
{
    [ServiceBundle("v1/api/wordbook/{method}")]
    public interface IWordbookAppService : IServiceKey
    {
        Task<string> Create(CreateWordbookInput input);

        Task<string> Update(UpdateWordbookInput input);

        Task<string> Delete(DeleteByIdInput input);

        Task<IPagedResult<GetWordbookOutput>> Query(QueryWordbookInput query);

        [ServiceRoute("{wordbookId}")]
        Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItems(long wordbookId);
    }
}
