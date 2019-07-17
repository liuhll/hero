using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Auth.IApplication.User.Dtos;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.User
{
    [ServiceBundle("v1/api/user")]
    public interface IUserAppService : IServiceKey
    {
        Task<string> CreateUser(CreateUserInput input);
    }
}
