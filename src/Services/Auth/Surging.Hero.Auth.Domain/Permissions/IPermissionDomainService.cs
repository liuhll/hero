using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.Role.Dtos;

namespace Surging.Hero.Auth.Domain.Permissions
{
    public interface IPermissionDomainService : ITransientDependency
    {
        Task<IEnumerable<GetRolePermissionTreeOutput>> GetRolePermissions(long roleId);

        Task<bool> Check(long userId, string serviceId);
    }
}
