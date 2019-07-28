using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.Domain.PagedAndSorted.Extensions;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.BasicData.Domain.Wordbooks;
using Surging.Hero.BasicData.IApplication.Wordbook;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;
using Surging.Hero.Common.Dtos;

namespace Surging.Hero.BasicData.Application.Wordbook
{
    public class WordbookAppService : ProxyServiceBase, IWordbookAppService
    {
        private readonly IWordbookDomainService _wordbookDomainService;

        public WordbookAppService(IWordbookDomainService wordbookDomainService)
        {
            _wordbookDomainService = wordbookDomainService;
        }

        public async Task<string> Create(CreateWordbookInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _wordbookDomainService.CreateWordbook(input);
            return "新增字典类型成功";
        }

        public async Task<string> CreateWordbookItem(CreateWordbookItemInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _wordbookDomainService.CreateWordbookItem(input);
            return "新增字典项成功";
        }

        public async Task<string> Delete(DeleteByIdInput input)
        {
            await _wordbookDomainService.DeleteWordbook(input.Id);
            return "删除字典类型成功";
        }

        public async Task<string> DeleteWordbookItem(DeleteByIdInput input)
        {
            await _wordbookDomainService.DeleteWordbookItem(input.Id);
            return "删除字典项目成功";
        }

        public async Task<GetWordbookOutput> Get(long id)
        {
            return (await _wordbookDomainService.GetWordbook(id)).MapTo<GetWordbookOutput>();
        }

        public async Task<GetWordbookItemOutput> GetWordbookItem(long id)
        {
            return (await _wordbookDomainService.GetWordbookItem(id)).MapTo<GetWordbookItemOutput>();
        }

        public async Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItems(long wordbookId)
        {
            return await _wordbookDomainService.GetWordbookItems(wordbookId);
        }

        public async Task<IPagedResult<GetWordbookOutput>> Query(QueryWordbookInput query)
        {
            var queryResult = await _wordbookDomainService.QueryWordbooks(query);
            return queryResult.Item1.MapTo<IEnumerable<GetWordbookOutput>>().GetPagedResult(queryResult.Item2);
        }

        public async Task<string> Update(UpdateWordbookInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _wordbookDomainService.UpdateWordbook(input);
            return "修改字典类型成功";
        }

        public async Task<string> UpdateWordbookItem(UpdateWordbookItemInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _wordbookDomainService.UpdateWordbookItem(input);
            return "修改字典项成功";
        }
    }
}
