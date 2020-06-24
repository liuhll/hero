using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.Action.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Hero.Auth.Domain.Permissions.Actions
{
    public interface IActionDomainService : ITransientDependency
    {
        Task InitActions(ICollection<InitActionActionInput> actions);
        Task<IEnumerable<Action>> GetOperationOutputActions(long id);
        Task<IEnumerable<GetServiceHostOutput>> GetServiceHosts(QueryServiceHostInput query);
        Task<IEnumerable<GetAppServiceOutput>> GetAppServices(QueryAppServiceInput query);
        Task<IEnumerable<GetActionOutput>> GetActionServices(QueryActionInput query);
        Task<IEnumerable<GetTreeActionOutput>> GetServicesTree();
    }
}
