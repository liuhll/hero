using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.Authorization
{
    [ServiceBundle("v1/api/account")]
    public interface IAccountAppService : IServiceKey
    {
        Task<string> Login(LoginInput input);
    }
}
