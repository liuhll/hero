using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;

namespace Surging.Hero.Auth.Domain.Permissions
{
    public interface IPermissionDomainService : ITransientDependency
    {
        Task<IDictionary<string,object>> Check(long userId, string serviceId);
    }
}