using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Auth.IApplication.Role.Dtos;

namespace Surging.Hero.Auth.Domain.Roles
{
    public interface IRoleDomainService : ITransientDependency
    {
        Task Create(CreateRoleInput input);
        Task Update(UpdateRoleInput input);
        Task UpdateStatus(UpdateRoleStatusInput input);

        Task RemoveRoleCheckPemissionCache(long roleId);
        
        Task<IEnumerable<RolePermission>> GetRolePermissions(long roleId);
        Task SetPermissions(SetRolePermissionInput input);
        Task Delete(long roleid);
        Task<bool> CheckPermission(long roleId, string serviceId);
        Task<GetRoleOutput> Get(long id);
        Task<IPagedResult<GetRoleOutput>> Search(QueryRoleInput query);
    }
}