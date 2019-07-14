using System.Threading.Tasks;
using Surging.Core.ProxyGenerator;
using Surging.Hero.Auth.IApplication.Authorization;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;

namespace Surging.Hero.Auth.Application.Authorization
{
    public class AccountAppService : ProxyServiceBase, IAccountAppService
    {
        public async Task<string> Login(LoginInput input)
        {
            return "token";
        }
    }
}
