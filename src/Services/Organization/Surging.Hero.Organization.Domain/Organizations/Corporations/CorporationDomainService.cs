using System;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.Auth.IApplication.User;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;

namespace Surging.Hero.Organization.Domain.Organizations
{
    public class CorporationDomainService : ManagerBase, ICorporationDomainService
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
            var thisLevelCorporationCount = await _corporationRepository.GetCountAsync(p => p.ParentId == input.ParentId);
            if (input.ParentId == 0)
            {
                await CreateTopCorporation(input, thisLevelCorporationCount);
            }
            else {
                await CreateSubCorporation(input, thisLevelCorporationCount);
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
            var corporationUsers = await GetService<IUserAppService>().GetCorporationUser(corporation.Id);
            if (corporationUsers.Any())
            {
                throw new BusinessException($"请先删除该公司下的用户");
            }
            await _corporationRepository.DeleteAsync(corporation);
        }

        public async Task<Corporation> GetCorporation(long id)
        {
            return await _corporationRepository.GetAsync(id);
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

        private async Task CreateSubCorporation(CreateCorporationInput input, int thisLevelCorporationCount)
        {
            var parentCorporation = await _corporationRepository.GetAsync(input.ParentId);
            if (parentCorporation.Mold == Shared.CorporationMold.Monomer)
            {
                throw new BusinessException("单体公司不允许增加子公司");
            }
            var corporation = input.MapTo<Corporation>();
            corporation.Code = parentCorporation.Code + HeroConstants.CodeRuleRestrain.CodeSeparator + (thisLevelCorporationCount + 1).ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
            corporation.Level = parentCorporation.Level + 1;
            await _corporationRepository.InsertAsync(corporation);
        }

        private async Task CreateTopCorporation(CreateCorporationInput input, int thisLevelCorporationCount)
        {
            Corporation topCorporation = await _corporationRepository.SingleOrDefaultAsync(p => p.ParentId == 0);
            if (topCorporation != null)
            {
                throw new BusinessException("系统中已经存在母公司,请不要重复添加");
            }
            if (input.Mold != Shared.CorporationMold.Group && input.Mold != Shared.CorporationMold.Monomer) {
                throw new BusinessException("公司类型不正确,顶层公司只能指定为:集团公司或单体公司");
            }
            topCorporation = input.MapTo<Corporation>();
            topCorporation.Code = (thisLevelCorporationCount + 1).ToString().PadRight(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
            topCorporation.Level = 1;
            await _corporationRepository.InsertAsync(topCorporation);




        }
    }
}
