using Surging.Core.CPlatform.Ioc;
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
    }
}
