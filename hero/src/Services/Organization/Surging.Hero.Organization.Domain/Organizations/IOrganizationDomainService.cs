using Surging.Core.CPlatform.Ioc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Organization.Domain
{
    public interface IOrganizationDomainService : ITransientDependency
    {
        Task<IEnumerable<Organization>> GetOrganizations();
    }
}
