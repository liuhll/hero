using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.PagedAndSorted;
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
        [ServiceRoute("create")]
        Task<string> Create(CreateWordbookInput input);

        [HttpPut(true)]
        [ServiceRoute("update")]
        Task<string> Update(UpdateWordbookInput input);

        [ServiceRoute("delete/{id}")]
        [HttpDelete(true)]
        Task<string> Delete(long id);


        [ServiceRoute("query")]
        Task<IPagedResult<GetWordbookOutput>> Query(QueryWordbookInput query);

        [ServiceRoute("get/{id}")]
        [HttpGet(true)]
        Task<GetWordbookOutput> Get(long id);

        [ServiceRoute("items/{wordbookId}")]
        [HttpGet(true)]
        Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItems(long wordbookId);

        [HttpPost(true)]
        [ServiceRoute("items/create")]
        Task<string> CreateWordbookItem(CreateWordbookItemInput input);

        [HttpPut(true)]
        [ServiceRoute("items/update")]
        Task<string> UpdateWordbookItem(UpdateWordbookItemInput input);

        [ServiceRoute("items/delete/{id}")]
        [HttpDelete(true)]
        Task<string> DeleteWordbookItem(long id);

        [ServiceRoute("items/get/{id}")]
        [HttpGet(true)]
        Task<GetWordbookItemOutput> GetWordbookItem(long id);

        [HttpPost(true)]
        [ServiceRoute("check")]
        Task<bool> Check(CheckWordbookInput input);

        [ServiceRoute("items/{code}")]
        [HttpGet(true)]
        Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItemByCode(string code);
    }
}
