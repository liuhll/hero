using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;
using Surging.Hero.Common.Maintenance;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.IApplication.Authorization
{
    [ServiceBundle("v1/api/account")]
    public interface IAccountAppService : IServiceKey
    {
        [Service(Director = Maintainer.Liuhll, Date = "2019-07-14", Name = "用户登录接口")]
        Task<LoginResult> Login(LoginInput input);
    }
}
