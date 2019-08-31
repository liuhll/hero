using Surging.Core.CPlatform.Ioc;
using Surging.Hero.Organization.Domain.Shared.Organizations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.Organization.Domain
{
    public interface IOrganizationDomainService : ITransientDependency
    {
        Task<IEnumerable<Organization>> GetOrganizations();
        Task<IEnumerable<Organization>> GetOrganizations(long? id, OrganizationType organizationType);
    }
}
