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
    }
}
