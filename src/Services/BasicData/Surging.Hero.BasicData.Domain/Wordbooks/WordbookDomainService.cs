using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.BasicData.Domain.Shared.Wordbooks;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;

namespace Surging.Hero.BasicData.Domain.Wordbooks
{
    public class WordbookDomainService : IWordbookDomainService
    {
        private readonly IDapperRepository<Wordbook, long> _wordbookRepository;
        private readonly IDapperRepository<WordbookItem, long> _wordbookItemRepository;
        public WordbookDomainService(IDapperRepository<Wordbook, long> wordbookRepository,
            IDapperRepository<WordbookItem, long> wordbookItemRepository) {
            _wordbookRepository = wordbookRepository;
            _wordbookItemRepository = wordbookItemRepository;
        }

        public async Task<bool> CheckWordbookItem(CheckWordbookInput input)
        {
            var wordbook = await _wordbookRepository.SingleOrDefaultAsync(p => p.Code == input.WordbookCode);
            if (wordbook == null) {
                throw new BusinessException($"系统中不存在Code为{input.WordbookCode}的字典项");
            }
            var wordbookItems = await _wordbookItemRepository.GetAllAsync(p => p.WordbookId == wordbook.Id);
            if (wordbookItems.Any(p => p.Id == input.WordbookItemId)) {
                return true;
            }
            return false;
        }

        public async Task CreateWordbook(CreateWordbookInput input)
        {
            var wordbook = await _wordbookRepository.FirstOrDefaultAsync(p => p.Code == input.Code);
            if (wordbook != null) {
                throw new BusinessException($"系统中已经存在code为{input.Code}的字典类型");
            }
            wordbook = input.MapTo<Wordbook>();
            await _wordbookRepository.InsertAsync(wordbook);
        }

        public async Task CreateWordbookItem(CreateWordbookItemInput input)
        {
            var wordbook = await _wordbookRepository.SingleOrDefaultAsync(p => p.Id == input.WordbookId);
            if (wordbook == null)
            {
                throw new BusinessException($"系统中不存在Id为{input.WordbookId}的字典类型");
            }
            var wordbookItem = await _wordbookItemRepository.SingleOrDefaultAsync(p => p.Key == input.Key && p.WordbookId == input.WordbookId);
            if (wordbookItem != null)
            {
                throw new BusinessException($"{wordbook.Name}已经存在key为{input.Key}的字典项");
            }
            wordbookItem = input.MapTo<WordbookItem>();
            await _wordbookItemRepository.InsertAsync(wordbookItem);
        }

        public async Task DeleteWordbook(long id)
        {
            var wordbook = await _wordbookRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (wordbook == null)
            {
                throw new BusinessException($"系统中不存在Id为{id}的字典类型");
            }
       
            if (wordbook.IsSysPreset)
            {
                throw new BusinessException($"不允许删除系统预设的字典类型");
            }
            await _wordbookRepository.DeleteAsync(wordbook);
        }

        public async Task DeleteWordbookItem(long id)
        {
            var wordbookItem = await _wordbookItemRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (wordbookItem == null)
            {
                throw new BusinessException($"系统中不存在Id为{id}的字典项");
            }
          
            await _wordbookItemRepository.DeleteAsync(wordbookItem);
        }

        public async Task<Wordbook> GetWordbook(long id)
        {
            var wordbook = await _wordbookRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (wordbook == null)
            {
                throw new BusinessException($"系统中不存在Id为{id}的字典类型");
            }
            return wordbook;
        }

        public async Task<GetWordbookItemOutput> GetWordbookItem(long id)
        {
            var wordbookItem = await _wordbookItemRepository.GetAsync(id);     
            var wordbook = await _wordbookRepository.SingleOrDefaultAsync(p => p.Id == wordbookItem.WordbookId);
            if (wordbook == null)
            {
                throw new BusinessException($"系统中不存在Id为{wordbookItem.WordbookId}的字典类型");
            }
            var wordbookItemOutput = wordbookItem.MapTo<GetWordbookItemOutput>();
            wordbookItemOutput.WordbookCode = wordbook.Code;
            return wordbookItemOutput;
        }

        public async Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItems(long wordbookId)
        {
            var wordbook = await _wordbookRepository.SingleOrDefaultAsync(p => p.Id == wordbookId);
            if (wordbook == null)
            {
                throw new BusinessException($"系统中不存在Id为{wordbookId}的字典类型");
            }
            var wordbookItems = await _wordbookItemRepository.GetAllAsync(p => p.WordbookId == wordbookId);
            var wordbookItemOutputs = wordbookItems.MapTo<IEnumerable<GetWordbookItemOutput>>().Select(p => { p.WordbookCode = wordbook.Code; return p; }).OrderBy(p=>p.Sort);
            return wordbookItemOutputs;
        }

        public async Task<Tuple<IEnumerable<Wordbook>, int>> QueryWordbooks(QueryWordbookInput query)
        {
            var queryResult = await _wordbookRepository.GetPageAsync(p => p.Name.Contains(query.SearchKey) || p.Code.Contains(query.SearchKey) || p.Memo.Contains(query.SearchKey),query.PageIndex,query.PageCount);
            return queryResult;
        }

        public async Task UpdateWordbook(UpdateWordbookInput input)
        {
            var wordbook = await _wordbookRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (wordbook == null)
            {
                throw new BusinessException($"系统中不存在Id为{input.Id}的字典类型");            
            }
            if (wordbook.IsSysPreset)
            {
                throw new BusinessException($"不允许修改系统预设的字典类型");
            }
            wordbook = input.MapTo(wordbook);
            await _wordbookRepository.UpdateAsync(wordbook);
        }

        public async Task UpdateWordbookItem(UpdateWordbookItemInput input)
        {
            var wordbookItem = await _wordbookItemRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (wordbookItem == null)
            {
                throw new BusinessException($"系统中不存在Id为{input.Id}的字典项");
            }
            var wordbook = await _wordbookItemRepository.SingleOrDefaultAsync(p => p.Id == wordbookItem.WordbookId);
            if (wordbookItem == null)
            {
                throw new BusinessException($"系统中不存在Id为{wordbookItem.WordbookId}的字典类型");
            }
            wordbookItem = input.MapTo(wordbookItem);
            await _wordbookItemRepository.UpdateAsync(wordbookItem);
        }
    }
}
