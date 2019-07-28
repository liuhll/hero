using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;

namespace Surging.Hero.BasicData.Domain.Wordbooks
{
    public interface IWordbookDomainService : ITransientDependency
    {
        Task CreateWordbook(CreateWordbookInput input);
        Task UpdateWordbook(UpdateWordbookInput input);
        Task DeleteWordbook(long id);
        Task<Tuple<IEnumerable<Wordbook>, int>> QueryWordbooks(QueryWordbookInput query);
        Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItems(long id);
        Task<Wordbook> GetWordbook(long wordbookId);
        Task CreateWordbookItem(CreateWordbookItemInput input);
    }
}
