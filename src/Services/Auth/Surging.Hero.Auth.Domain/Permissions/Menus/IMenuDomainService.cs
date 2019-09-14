using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.Permissions.Menus
{
    public interface IMenuDomainService : ITransientDependency
    {
        Task Create(CreateMenuInput input);
    }
}
