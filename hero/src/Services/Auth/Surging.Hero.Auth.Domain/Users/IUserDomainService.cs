using Surging.Core.CPlatform.Ioc;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.Users
{
    public interface IUserDomainService : ITransientDependency
    {
        Task CreateUser(UserInfo userInfo);
        Task ResetPassword(UserInfo userInfo, string newPassword);
    }
}
