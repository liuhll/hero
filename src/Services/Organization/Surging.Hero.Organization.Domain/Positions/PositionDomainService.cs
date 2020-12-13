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
using Surging.Hero.Organization.IApplication.Position;
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

        public async Task CreatePosition(Position position, string positionCode, DbConnection conn, DbTransaction trans)
        {
            position.Code = positionCode;
            await _positionRepository.InsertAsync(position, conn, trans);
        }


        public async Task DeletePosition(long id)
        {
            var position = await _positionRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (position == null) {
                throw new BusinessException($"不存在Id为{id}的职位信息");
            }
            await _positionRepository.DeleteAsync(position);
        }

        public async Task<IEnumerable<GetPositionOutput>> GetPositionsByDeptId(long deptId)
        {
            var positions = await _positionRepository.GetAllAsync(p => p.DeptId == deptId);
            var positionOutputs = positions.MapTo<IEnumerable<GetPositionOutput>>();
            foreach (var positionOutput in positionOutputs)
            {
                var wordbookAppServiceProxy = GetService<IWordbookAppService>();
                var positionLevleInfo = await wordbookAppServiceProxy.GetWordbookItemByKey(
                    OrganizationConstant.PositionPositionLevelWordbookCode, positionOutput.PositionLevelKey);
                if (positionLevleInfo != null)
                {
                    positionOutput.PositionLevelName = positionLevleInfo.Value;
                }
                var positionFunctionInfo = await wordbookAppServiceProxy.GetWordbookItemByKey(
                    OrganizationConstant.PositionPositionLevelWordbookCode, positionOutput.PositionLevelKey);
                if (positionFunctionInfo != null)
                {
                    positionOutput.FunctionName = positionFunctionInfo.Value;
                }
            }
            return positionOutputs;
        }

        public async Task UpdatePosition(UpdatePositionInput input)
        {
            var position = await _positionRepository.GetAsync(input.Id);          
            var workbookAppServiceProxy = GetService<IWordbookAppService>();
            if (!await workbookAppServiceProxy.Check(new CheckWordbookInput() { WordbookCode = SystemPresetWordbookCode.Organization.PositionFunction, WordbookItemKey = input.FunctionKey }))
            {
                throw new BusinessException($"系统中不存在指定的岗位职能类型");
            }
            if (!await workbookAppServiceProxy.Check(new CheckWordbookInput() { WordbookCode = SystemPresetWordbookCode.Organization.PositionLevel, WordbookItemKey = input.PositionLevelKey }))
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

        private async Task CheckPosition(Position position)
        {         
            var workbookAppServiceProxy = GetService<IWordbookAppService>();
            if (!await workbookAppServiceProxy.Check(new CheckWordbookInput() { WordbookCode = SystemPresetWordbookCode.Organization.PositionFunction, WordbookItemKey = position.FunctionKey }))
            {
                throw new BusinessException($"系统中不存在指定的岗位职能类型");
            }
            if (!await workbookAppServiceProxy.Check(new CheckWordbookInput() { WordbookCode = SystemPresetWordbookCode.Organization.PositionLevel, WordbookItemKey = position.PositionLevelKey }))
            {
                throw new BusinessException($"系统中不存在指定的岗位级别");
            }

        }
    }
}
