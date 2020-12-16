using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.Permission.Dtos;

namespace Surging.Hero.Auth.Domain.Permissions.Operations
{
    public interface IOperationDomainService : ITransientDependency
    {
        Task<CreateOperationOutput> Create(CreateOperationInput input);
        Task<UpdateOperationOutput> Update(UpdateOperationInput input);
        Task Delete(long permissionId);
        Task<IEnumerable<Operation>> GetAll();
        Task<bool> CheckPermission(long operationId, string serviceId);
    }
}