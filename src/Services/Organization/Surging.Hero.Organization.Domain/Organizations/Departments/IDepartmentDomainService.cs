using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Hero.Organization.IApplication.Department.Dtos;

namespace Surging.Hero.Organization.Domain.Organizations.Departments
{
    public interface IDepartmentDomainService : ITransientDependency
    {
        Task<CreateDepartmentOutput> CreateDepartment(CreateDepartmentInput input);
        Task<UpdateDepartmentOutput> UpdateDepartment(UpdateDepartmentInput input);
        Task DeleteDepartmentByOrgId(long orgId);
        Task<GetDepartmentOutput> GetDepartment(long id);
        Task<GetDepartmentOutput> GetDepartmentByOrgId(long orgId);
    }
}