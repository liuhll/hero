using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public interface IUserGroupDomainService : ITransientDependency
    {
        Task Create(CreateUserGroupInput input);
        Task Update(UpdateUserGroupInput input);
        Task Delete(long id);
        Task<IEnumerable<GetDisplayRoleOutput>> GetUserGroupRoles(long userGroupId);
        Task<IEnumerable<GetUserBasicOutput>> GetUserGroupUsers(long userGroupId);
        Task<bool> CheckPermission(long userId, string serviceId);
    }
}
