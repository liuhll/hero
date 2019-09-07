using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.IApplication.User;
using Surging.Hero.BasicData.Domain.Shared.Wordbooks;
using Surging.Hero.BasicData.IApplication.Wordbook;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;
using Surging.Hero.Common;
using Surging.Hero.Organization.Domain.Positions;
using Surging.Hero.Organization.IApplication.Department.Dtos;
using Surging.Hero.Organization.IApplication.Position.Dtos;

namespace Surging.Hero.Organization.Domain.Organizations.Departments
{
    public class DepartmentDomainService : ManagerBase, IDepartmentDomainService
    {
        private readonly IDapperRepository<Department, long> _departmentRepository;
        private readonly IDapperRepository<Corporation, long> _corporationRepository;
        private readonly IDapperRepository<Position, long> _positionRepository;
        private readonly IPositionDomainService _positionDomainService;
    
        public DepartmentDomainService(IDapperRepository<Department, long> departmentRepository,
            IDapperRepository<Corporation, long> corporationRepository,
            IDapperRepository<Position, long> positionRepository,
            IPositionDomainService positionDomainService)
        {
            _departmentRepository = departmentRepository;
            _corporationRepository = corporationRepository;
            _positionRepository = positionRepository;
            _positionDomainService = positionDomainService;
           
        }

        public async Task CreateDepartment(CreateDepartmentInput input)
        {
            var corporation = await _corporationRepository.SingleOrDefaultAsync(p => p.Id == input.CorporationId);
            if (corporation == null)
            {
                throw new BusinessException($"系统中不存在Id为{input.CorporationId}的企业信息");
            }       
   
            if (input.DeptTypeId == 0)
            {
                throw new BusinessException($"请选择部门类型");
            }
            var workbookAppServiceProxy = GetService<IWordbookAppService>();
            var checkDeptTypeResult = await workbookAppServiceProxy.Check(new CheckWordbookInput() { WordbookCode = SystemPresetWordbookCode.Organization.DeptType, WordbookItemId = input.DeptTypeId });
            if (!checkDeptTypeResult)
            {
                throw new BusinessException($"部门类型Id不正确,请选择正确的部门类型");
            }
            var department = input.MapTo<Department>();
            var thisDepartmentCount = await _departmentRepository.GetCountAsync(p => p.CorporationId == input.CorporationId);

            if (input.ParentId == 0)
            {
                department.Level = 1;
                department.Code = corporation.Code + HeroConstants.CodeRuleRestrain.CodeSeparator + (thisDepartmentCount + 1).ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
            }
            else
            {
                var parentDept = await _departmentRepository.SingleOrDefaultAsync(p => p.Id == input.ParentId);
                if (parentDept == null)
                {
                    throw new BusinessException($"系统中不存在Id为{input.ParentId}的父级部门信息");
                }
                department.Level = parentDept.Level + 1;
                department.Code = parentDept.Code + HeroConstants.CodeRuleRestrain.CodeSeparator + (thisDepartmentCount + 1).ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);

            }

            await UnitOfWorkAsync(async (conn, trans) =>
            {
                var deptId = await _departmentRepository.InsertAndGetIdAsync(department, conn, trans);
                if (input.Postions != null && input.Postions.Any())
                {
                    if (input.Postions.Count(p => p.IsLeadingOfficial) > 1) {
                        throw new BusinessException($"部门只允许设置一个负责人岗位");
                    }
                    var sort = 1;
                    foreach (var position in input.Postions)
                    {
                        position.DeptId = deptId;
                        position.CheckDataAnnotations().CheckValidResult();
                        var positionCode = department.Code + HeroConstants.CodeRuleRestrain.CodeSeparator + sort.ToString().PadLeft(HeroConstants.CodeRuleRestrain.CodeCoverBit, HeroConstants.CodeRuleRestrain.CodeCoverSymbol);
                        await _positionDomainService.CreatePosition(position, positionCode, conn, trans);
                        sort++;
                    }
                }
            }, Connection);
        }

        public async Task DeleteDepartment(long id)
        {
            var department = await _departmentRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (department == null)
            {
                throw new BusinessException($"系统中不存在Id为{id}的部门信息");
            }
            var children = await _departmentRepository.GetAllAsync(p => p.ParentId == id);
            if (children.Any()) {
                throw new BusinessException($"请先删除子部门信息");
            }
            var departmentUsers = await GetService<IUserAppService>().GetDepartmentUser(department.Id);
            if (departmentUsers.Any()) {
                throw new BusinessException($"该部门存在用户,请先删除该部门下的用户");
            }
            await UnitOfWorkAsync(async (conn, trans) => {
                await _departmentRepository.DeleteAsync(department,conn,trans);
                await _positionRepository.DeleteAsync(p => p.Code.Contains(department.Code), conn, trans);
            },Connection);
            
        }

        public async Task<GetDepartmentOutput> GetDepartment(long id)
        {
            var department = await _departmentRepository.SingleOrDefaultAsync(p => p.Id == id);
            if (department == null)
            {
                throw new BusinessException($"系统中不存在Id为{id}的部门信息");
            }
            var departmentOutput = department.MapTo<GetDepartmentOutput>();
            var positions = await _positionDomainService.GetPositionsByDeptId(department.Id);
            departmentOutput.Postions = positions.MapTo<IEnumerable<GetPositionOutput>>();
            return departmentOutput;
        }

        public async Task UpdateDepartment(UpdateDepartmentInput input)
        {
            var department = await _departmentRepository.SingleOrDefaultAsync(p => p.Id == input.Id);
            if (department == null) {
                throw new BusinessException($"系统中不存在Id为{input.Id}的部门信息");
            }
            if (input.DeptTypeId == 0)
            {
                throw new BusinessException($"请选择部门类型");
            }
            var workbookAppServiceProxy = GetService<IWordbookAppService>();
            var checkDeptTypeResult = await workbookAppServiceProxy.Check(new CheckWordbookInput() { WordbookCode = SystemPresetWordbookCode.Organization.DeptType, WordbookItemId = input.DeptTypeId });
            if (!checkDeptTypeResult)
            {
                throw new BusinessException($"部门类型Id不正确,请选择正确的部门类型");
            }            
            department = input.MapTo(department);
            await _departmentRepository.UpdateAsync(department);
        }
    }
}
