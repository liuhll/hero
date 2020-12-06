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
        Task Delete(long permissionId);

        Task<IEnumerable<Menu>> GetAll();
        Task<IEnumerable<GetPermissionTreeOutput>> GetTree();

        Task<IEnumerable<Menu>> GetParents(long menuId, bool isIncludeSelf = true);
        Task<IEnumerable<Menu>> GetParentsByPermissionId(long permissionId, Shared.Permissions.PermissionMold mold, bool isIncludeSelf = true);
    }
}
