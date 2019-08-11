using System.Threading.Tasks;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Organization.Domain.Organizations.Departments;
using Surging.Hero.Organization.IApplication.Department;
using Surging.Hero.Organization.IApplication.Department.Dtos;

namespace Surging.Hero.Organization.Application.Department
{
    public class DepartmentAppService : ProxyServiceBase, IDepartmentAppService
    {
        private readonly IDepartmentDomainService _departmentDomainService;

        public DepartmentAppService(IDepartmentDomainService departmentDomainService) {
            _departmentDomainService = departmentDomainService;
        }

        public async Task<string> Create(CreateDepartmentInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _departmentDomainService.CreateDepartment(input);
            return "新增部门信息成功";
        }

        public async Task<string> Update(UpdateDepartmentInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _departmentDomainService.UpdateDepartment(input);
            return "更新部门信息成功";
        }
    }
}
