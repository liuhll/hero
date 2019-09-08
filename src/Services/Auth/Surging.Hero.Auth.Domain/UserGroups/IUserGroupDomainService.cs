using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.UserGroup.Dtos;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public interface IUserGroupDomainService : ITransientDependency
    {
        Task Create(CreateUserGroupInput input);
        Task Update(UpdateUserGroupInput input);
    }
}
