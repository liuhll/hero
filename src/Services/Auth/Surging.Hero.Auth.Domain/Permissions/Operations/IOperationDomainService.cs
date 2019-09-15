using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.Permission.Dtos;

namespace Surging.Hero.Auth.Domain.Permissions.Operations
{
    public interface IOperationDomainService : ITransientDependency
    {
        Task Create(CreateOperationInput input);
    }
}
