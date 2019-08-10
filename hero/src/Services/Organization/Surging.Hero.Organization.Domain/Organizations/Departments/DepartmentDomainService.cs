using System.Linq;
using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.Dapper.Manager;
using Surging.Core.Dapper.Repositories;
using Surging.Core.Validation.DataAnnotationValidation;
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
            IPositionDomainService positionDomainService) {
            _departmentRepository = departmentRepository;
            _corporationRepository = corporationRepository;
            _positionDomainService = positionDomainService;
        }

        public async Task CreateDepartment(CreateDepartmentInput input)
        {
            var corporation = await _corporationRepository.SingleOrDefaultAsync(p => p.Id == input.CorporationId);
            if (corporation == null) {
                throw new BusinessException($"系统中不存在Id为{input.CorporationId}的企业信息");
            }
            if (input.ParentId != 0) {
                var parentDept = await _departmentRepository.SingleOrDefaultAsync(p => p.Id == input.ParentId);
                if (parentDept == null)
                {
                    throw new BusinessException($"系统中不存在Id为{input.ParentId}的父级部门信息");
                }
            }
            var department = await _departmentRepository.SingleOrDefaultAsync(p => p.Code == input.Code);
            if (department != null) {
                throw new BusinessException($"系统中已经存在Code为{input.Code}的部门信息");
            }

            department = input.MapTo<Department>();
            await UnitOfWorkAsync(async (conn, trans) => {
                var deptId = await _departmentRepository.InsertAndGetIdAsync(department,conn,trans);
                if (input.Postions != null && input.Postions.Any()) {
                    foreach (var position in input.Postions) {
                        position.CheckDataAnnotations().CheckValidResult();
                        position.DeptId = deptId;
                        await _positionDomainService.CreatePosition(position, conn, trans);
                    }
                }
            }, Connection);
        }
    }
}
