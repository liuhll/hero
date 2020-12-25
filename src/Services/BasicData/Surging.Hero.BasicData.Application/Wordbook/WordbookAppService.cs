using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.CPlatform.Exceptions;
using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Cloud.Domain.PagedAndSorted.Extensions;
using Surging.Cloud.ProxyGenerator;
using Surging.Cloud.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.IApplication.FullAuditDtos;
using Surging.Hero.Auth.IApplication.User;
using Surging.Hero.BasicData.Domain.Wordbooks;
using Surging.Hero.BasicData.IApplication.Wordbook;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;

namespace Surging.Hero.BasicData.Application.Wordbook
{
    public class WordbookAppService : ProxyServiceBase, IWordbookAppService
    {
        private readonly IWordbookDomainService _wordbookDomainService;

        public WordbookAppService(IWordbookDomainService wordbookDomainService)
        {
            _wordbookDomainService = wordbookDomainService;
        }

        public async Task<bool> Check(CheckWordbookInput input)
        {
            return await _wordbookDomainService.CheckWordbookItem(input);
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

        public async Task<string> Delete(long id)
        {
            await _wordbookDomainService.DeleteWordbook(id);
            return "删除字典类型成功";
        }

        public async Task<string> DeleteWordbookItem(long id)
        {
            await _wordbookDomainService.DeleteWordbookItem(id);
            return "删除字典项目成功";
        }

        public async Task<GetWordbookOutput> Get(long id)
        {
            var wordbookOutput = (await _wordbookDomainService.GetWordbook(id)).MapTo<GetWordbookOutput>();
            await wordbookOutput.SetAuditInfo();
            return wordbookOutput;
        }

        public async Task<GetWordbookItemOutput> GetWordbookItem(long id)
        {
            var wordbookOutput = (await _wordbookDomainService.GetWordbookItem(id)).MapTo<GetWordbookItemOutput>();
            await wordbookOutput.SetAuditInfo();
            return wordbookOutput;
        }

        public async Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItemsByCode(string code)
        {
            var wordbookOutputs = await _wordbookDomainService.GetWordbookItemsByCode(code);
            foreach (var wordbookOutput in wordbookOutputs) await wordbookOutput.SetAuditInfo();
            return wordbookOutputs;
        }

        public async Task<GetWordbookItemOutput> GetWordbookItemByKey(string wordbookCode, string key)
        {
            var wordbookOutput = await _wordbookDomainService.GetWordbookItemByKey(wordbookCode, key);
            return wordbookOutput;
        }

        public async Task<IPagedResult<GetWordbookItemOutput>> GetWordbookItems(GetWordbookItemsInput input)
        {
            if (input.Code.IsNullOrEmpty() && !input.WordbookId.HasValue)
                throw new BusinessException("字典编码和字典Id不能同时为空");
            var outputs = await _wordbookDomainService.GetWordbookItems(input);
            foreach (var output in outputs.Items) await output.SetAuditInfo();
            return outputs;
        }

        public async Task<IPagedResult<GetWordbookOutput>> Search(QueryWordbookInput query)
        {
            var queryResult = await _wordbookDomainService.QueryWordbooks(query);
            var outputs = queryResult.Item1.MapTo<IEnumerable<GetWordbookOutput>>().GetPagedResult(queryResult.Item2);
            // todo 抽象
            foreach (var output in outputs.Items) await output.SetAuditInfo();
            return outputs;
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