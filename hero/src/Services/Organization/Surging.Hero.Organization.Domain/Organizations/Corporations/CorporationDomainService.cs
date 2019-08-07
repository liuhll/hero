using System;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;

namespace Surging.Hero.Organization.Domain.Organizations
{
    public class CorporationDomainService : ICorporationDomainService
    {
        private readonly IDapperRepository<Corporation, long> _corporationRepository;

        public CorporationDomainService(IDapperRepository<Corporation, long> corporationRepository)
        {
            _corporationRepository = corporationRepository;
        }


        public async Task CreateCorporation(CreateCorporationInput input)
        {
            var existCodeCorporation = await _corporationRepository.FirstOrDefaultAsync(p => p.Code == input.Code);
            if (existCodeCorporation != null)
            {
                throw new BusinessException($"系统中已经存在code为{input.Code}的企业信息");
            }
            if (input.ParentId == 0)
            {
                await CreateTopCorporation(input);
            }
            else {
                await CreateSubCorporation(input);
            }
        }

        private async Task CreateSubCorporation(CreateCorporationInput input)
        {
            
            var topCorporation = await _corporationRepository.SingleOrDefaultAsync(p => p.ParentId == 0);
            if (topCorporation == null)
            {
                throw new BusinessException("系统中不存在母公司,请先增加母公司");
            }
            //if (topCorporation.Type == Shared.CorporationType.Monomer)
            //{
            //    throw new BusinessException("单体公司不允许增加子公司");
            //}
            
            var parentCorporation = await _corporationRepository.GetAsync(input.ParentId);
            if (parentCorporation.Type == Shared.CorporationType.Monomer)
            {
                throw new BusinessException("单体公司不允许增加子公司");
            }
            var corporation = input.MapTo<Corporation>();
            await _corporationRepository.InsertAsync(corporation);
        }

        private async Task CreateTopCorporation(CreateCorporationInput input)
        {
            var topCorporation = await _corporationRepository.SingleOrDefaultAsync(p => p.ParentId == 0);
            if (topCorporation != null)
            {
                throw new BusinessException("系统中已经存在母公司,请不要重复添加");
            }
            topCorporation = input.MapTo<Corporation>();
            await _corporationRepository.InsertAsync(topCorporation);
                
        }
    }
}
