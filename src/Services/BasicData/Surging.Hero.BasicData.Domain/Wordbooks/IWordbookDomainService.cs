using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;

namespace Surging.Hero.BasicData.Domain.Wordbooks
{
    public interface IWordbookDomainService : ITransientDependency
    {
        Task CreateWordbook(CreateWordbookInput input);
        Task UpdateWordbook(UpdateWordbookInput input);
        Task DeleteWordbook(long id);
        Task<Tuple<IEnumerable<Wordbook>, int>> QueryWordbooks(QueryWordbookInput query);
        Task<IPagedResult<GetWordbookItemOutput>> GetWordbookItems(GetWordbookItemsInput input);
        Task<Wordbook> GetWordbook(long wordbookId);
        Task CreateWordbookItem(CreateWordbookItemInput input);
        Task UpdateWordbookItem(UpdateWordbookItemInput input);
        Task DeleteWordbookItem(long id);
        Task<GetWordbookItemOutput> GetWordbookItem(long id);
        Task<bool> CheckWordbookItem(CheckWordbookInput input);
        Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItemsByCode(string code);
        Task<GetWordbookItemOutput> GetWordbookItemByKey(string wordbookCode,string key);
    }
}
