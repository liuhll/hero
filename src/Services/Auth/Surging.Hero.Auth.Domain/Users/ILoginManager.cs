using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.Authorization.Dtos;

namespace Surging.Hero.Auth.Domain.Users
{
    public interface ILoginManager : ITransientDependency
    {
        Task<IDictionary<string, object>> Login(LoginInput input);
    }
}