using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Organization.IApplication.Department.Dtos;

namespace Surging.Hero.Organization.Domain.Organizations.Departments
{
    public interface IDepartmentDomainService : ITransientDependency
    {
        Task CreateDepartment(CreateDepartmentInput input);
        Task UpdateDepartment(UpdateDepartmentInput input);
    }
}
