using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Routing;
using Surging.Hero.Auth.Domain.Permissions.Menus;
using Surging.Hero.Auth.Domain.Permissions.Operations;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.IApplication.User.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.Users
{
    public interface IUserDomainService : ITransientDependency
    {
        Task Create(CreateUserInput input);
        Task ResetPassword(UserInfo userInfo, string newPassword);
        Task Update(UpdateUserInput input);
        Task Delete(long id);

        Task<IEnumerable<Role>> GetUserRoles(long userId);
        Task<GetUserNormOutput> GetUserNormInfoById(long id);
        Task<bool> CheckPermission(long userId, string serviceId);
        Task<IEnumerable<Menu>> GetUserMenu(long userId);
        Task<IEnumerable<Operation>> GetUserOperation(long userId, long menuId);
    }
}
