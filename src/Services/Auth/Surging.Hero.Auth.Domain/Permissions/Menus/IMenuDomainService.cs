using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Hero.Auth.Domain.Shared.Permissions;
using Surging.Hero.Auth.IApplication.Permission.Dtos;

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

        Task<IEnumerable<Menu>> GetParentsByPermissionId(long permissionId, PermissionMold mold,
            bool isIncludeSelf = true);
    }
}