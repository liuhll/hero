using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.Role.Dtos;

namespace Surging.Hero.Auth.Domain.Roles
{
    public interface IRoleDomainService : ITransientDependency
    {
        Task Create(CreateRoleInput input);
    }
}
