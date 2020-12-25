using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Auth.Domain.Roles;
using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public interface IUserGroupDomainService : ITransientDependency
    {
        Task Create(CreateUserGroupInput input);
        Task Update(UpdateUserGroupInput input);
        Task Delete(long id);
        Task<IEnumerable<GetDisplayRoleOutput>> GetUserGroupRoleOutputs(long userGroupId, Status? status = null);
        
        Task<IEnumerable<Role>> GetUserGroupRoles(long userGroupId, Status? status = null);
        Task<IEnumerable<GetUserBasicOutput>> GetUserGroupUsers(long userGroupId);
        Task<bool> CheckPermission(long userId, string serviceId);
        Task<string> AllocationUsers(AllocationUserIdsInput input);
        Task DeleteUserGroupUser(DeleteUserGroupUserInput input);
        Task<IPagedResult<GetUserNormOutput>> SearchUserGroupUser(QueryUserGroupUserInput query);
        Task UpdateStatus(UpdateUserGroupStatusInput input);

        Task<IEnumerable<UserGroupPermissionModel>> GetUserGroupPermissions(long userGroupId,
            Status status = Status.Valid);

        Task<IEnumerable<GetDisplayDataPermissionOrgOutput>> GetUserGroupDataPermissionOrgs(long userGroupId);
        Task<IPagedResult<GetUserGroupOutput>> Search(QueryUserGroupInput query);
    }
}