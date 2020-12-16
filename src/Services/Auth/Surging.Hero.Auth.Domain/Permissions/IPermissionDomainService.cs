using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;

namespace Surging.Hero.Auth.Domain.Permissions
{
    public interface IPermissionDomainService : ITransientDependency
    {
        Task<bool> Check(long userId, string serviceId);
    }
}