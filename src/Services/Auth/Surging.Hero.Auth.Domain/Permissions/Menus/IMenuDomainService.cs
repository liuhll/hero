using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.Permission.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.Permissions.Menus
{
    public interface IMenuDomainService : ITransientDependency
    {
        Task<CreateMenuOutput> Create(CreateMenuInput input);
        Task<UpdateMenuOutput> Update(UpdateMenuInput input);
        Task Delete(long id);

        Task<IEnumerable<Menu>> GetAll();
    }
}
