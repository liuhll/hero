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
using Surging.Hero.Organization.Domain.Positions;
using Surging.Hero.Organization.IApplication.Department.Dtos;

namespace Surging.Hero.Organization.Domain.Organizations.Departments
{
    public class DepartmentDomainService : ManagerBase, IDepartmentDomainService
    {
        private readonly IDapperRepository<Department, long> _departmentRepository;
        private readonly IDapperRepository<Corporation, long> _corporationRepository;
        private readonly IPositionDomainService _positionDomainService;

        public DepartmentDomainService(IDapperRepository<Department, long> departmentRepository,
            IDapperRepository<Corporation, long> corporationRepository,
            IPositionDomainService positionDomainService)
        {
            _departmentRepository = departmentRepository;
            _corporationRepository = corporationRepository;
            _positionDomainService = positionDomainService;
           
        }

        public async Task CreateDepartment(CreateDepartmentInput input)
        {
            var corporation = await _corporationRepository.SingleOrDefaultAsync(p => p.Id == input.CorporationId);
            if (corporation == null)
            {
                throw new BusinessException($"系统中不存在Id为{input.CorporationId}的企业信息");
            }
            if (input.ParentId != 0)
            {
                var parentDept = await _departmentRepository.SingleOrDefaultAsync(p => p.Id == input.ParentId);
                if (parentDept == null)
                {
                    throw new BusinessException($"系统中不存在Id为{input.ParentId}的父级部门信息");
                }
            }
            var department = await _departmentRepository.SingleOrDefaultAsync(p => p.Code == input.Code);
            if (department != null)
            {
                throw new BusinessException($"系统中已经存在Code为{input.Code}的部门信息");
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

            department = input.MapTo<Department>();
            await UnitOfWorkAsync(async (conn, trans) =>
            {
                var deptId = await _departmentRepository.InsertAndGetIdAsync(department, conn, trans);
                if (input.Postions != null && input.Postions.Any())
                {
                    if (input.Postions.Count(p => p.IsLeadingOfficial) > 1) {
                        throw new BusinessException($"部门只允许设置一个负责人岗位");
                    }
                    foreach (var position in input.Postions)
                    {
                        position.DeptId = deptId;
                        position.CheckDataAnnotations().CheckValidResult();
                        await _positionDomainService.CreatePosition(position, conn, trans);
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
            await _departmentRepository.DeleteAsync(department);
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
            if (input.Code != department.Code) {
                var exsitDepartment = await _departmentRepository.SingleOrDefaultAsync(p => p.Code == input.Code);
                if (exsitDepartment != null)
                {
                    throw new BusinessException($"系统中已经存在Code为{input.Code}的部门信息");
                }
            }
            department = input.MapTo(department);
            await _departmentRepository.UpdateAsync(department);
        }
    }
}
