using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Hero.BasicData.Domain.Shared.Wordbooks;
using Surging.Hero.BasicData.IApplication.Wordbook;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Organization.IApplication.Position.Dtos;

namespace Surging.Hero.Organization.Domain.Positions
{
    public class PositionDomainService : ManagerBase, IPositionDomainService
    {
        private readonly IDapperRepository<Position, long> _positionRepository;
        private readonly IDapperRepository<Department, long> _departmentRepository;
        private readonly IDapperRepository<Organization,long> _organizationRepository;

        public PositionDomainService(IDapperRepository<Position, long> positionRepository,
            IDapperRepository<Department, long> departmentRepository,
             IDapperRepository<Organization, long> organizationRepository) {
            _positionRepository = positionRepository;
            _departmentRepository = departmentRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task CreatePosition(CreatePositionInput input,string positionCode, DbConnection conn, DbTransaction trans)
        {
            await CheckPosition(input);
            var position = input.MapTo<Position>();
            position.Code = positionCode;
            await _positionRepository.InsertAsync(position, conn, trans);
        }

        public async Task CreatePosition(CreatePositionInput input)
        {
            await CheckPosition(input);            
            var position = input.MapTo<Position>();
            var departPositionMax = (await _positionRepository.GetAllAsync(p => p.DeptId == input.DeptId)).FirstOrDefault();
            var department = await _departmentRepository.GetAsync(input.DeptId);
            var orgInfo = await _organizationRepository.GetAsync(department.OrgId);
            var positionCode = string.Empty;
            if (departPositionMax == null)
            {
                positionCode = "1".PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
            }
            else
            {
                positionCode = (Convert.ToInt32(departPositionMax.Code.TrimStart('0')) + 1).ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
            }

            position.Code = orgInfo.Code + positionCode;
            await _positionRepository.InsertAsync(position);
        }

        public async Task DeletePosition(long id)
        {
            var position = await _positionRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (position == null) {
                throw new BusinessException($"不存在Id为{id}的职位信息");
            }
            await _positionRepository.DeleteAsync(position);
        }

        public async Task<IEnumerable<Position>> GetPositionsByDeptId(long deptId)
        {
            return await _positionRepository.GetAllAsync(p => p.DeptId == deptId);
        }

        public async Task UpdatePosition(UpdatePositionInput input)
        {
            var position = await _positionRepository.GetAsync(input.Id);          
            var workbookAppServiceProxy = GetService<IWordbookAppService>();
            if (!await workbookAppServiceProxy.Check(new CheckWordbookInput() { WordbookCode = SystemPresetWordbookCode.Organization.PositionFunction, WordbookItemId = input.FunctionId }))
            {
                throw new BusinessException($"系统中不存在指定的岗位职能类型");
            }
            if (!await workbookAppServiceProxy.Check(new CheckWordbookInput() { WordbookCode = SystemPresetWordbookCode.Organization.PositionLevel, WordbookItemId = input.PositionLevelId }))
            {
                throw new BusinessException($"系统中不存在指定的岗位级别");
            }
            if (input.IsLeadingOfficial && !position.IsLeadingOfficial)
            {
                var positions = await _positionRepository.GetAllAsync(p => p.DeptId == position.DeptId);
                if (positions.Any(p => p.IsLeadingOfficial))
                {
                    throw new BusinessException($"该部门已经设置负责人岗位,一个部门只允许设置一个负责人岗位");
                }
            }

            position = input.MapTo(position);
            await _positionRepository.UpdateAsync(position);
        }

        private async Task CheckPosition(CreatePositionInput input)
        {         
            var workbookAppServiceProxy = GetService<IWordbookAppService>();
            if (!await workbookAppServiceProxy.Check(new CheckWordbookInput() { WordbookCode = SystemPresetWordbookCode.Organization.PositionFunction, WordbookItemId = input.FunctionId }))
            {
                throw new BusinessException($"系统中不存在指定的岗位职能类型");
            }
            if (!await workbookAppServiceProxy.Check(new CheckWordbookInput() { WordbookCode = SystemPresetWordbookCode.Organization.PositionLevel, WordbookItemId = input.PositionLevelId }))
            {
                throw new BusinessException($"系统中不存在指定的岗位级别");
            }

        }
    }
}
