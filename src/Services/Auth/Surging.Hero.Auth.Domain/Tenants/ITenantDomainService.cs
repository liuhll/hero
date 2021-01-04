using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Hero.Auth.IApplication.Tenant.Dtos;

namespace Surging.Hero.Auth.Domain.Tenants
{
    public interface ITenantDomainService : ITransientDependency
    {
        Task<string> Create(CreateTenantInput input);
    }
}