using System;
using System.Linq;
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
        private readonly IDapperRepository<Department, long> _departmentRepository;
        
        public CorporationDomainService(IDapperRepository<Corporation, long> corporationRepository,
            IDapperRepository<Department, long> departmentRepository)
        {
            _corporationRepository = corporationRepository;
            _departmentRepository = departmentRepository;
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

        public async Task DeleteCorporation(long id)
        {
            var corporation = await _corporationRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (corporation == null)
            {
                throw new BusinessException($"系统中不存在Id为{id}的企业信息");
            }
            var children = await _corporationRepository.GetAllAsync(p => p.ParentId == id);
            if (children.Any())
            {
                throw new BusinessException($"请先删除子公司信息");
            }
            var departments = await _departmentRepository.GetAllAsync(p => p.CorporationId == id);
            if (departments.Any())
            {
                throw new BusinessException($"请先删除该公司的部门信息");
            }
            await _corporationRepository.DeleteAsync(corporation);
        }

        public async Task UpdateCorporation(UpdateCorporationInput input)
        {
            var corporation = await _corporationRepository.SingleAsync(p=>p.Id == input.Id);
            if (corporation == null) {
                throw new BusinessException($"系统中不存在Id为{input.Id}的企业信息");
            }
            corporation = input.MapTo(corporation);
            await _corporationRepository.UpdateAsync(corporation);

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
