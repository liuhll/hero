using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Hero.Auth.IApplication.Tenant.Dtos;

namespace Surging.Hero.Auth.Domain.Tenants
{
    public interface ITenantDomainService : ITransientDependency
    {
        Task<string> Create(CreateTenantInput input);
        
        Task<string> Update(UpdateTenantInput input);
        
        Task<string> Delete(long id);
        Task<IPagedResult<GetTenantOutput>> Search(QueryTenantInput query);
        Task<string> Status(UpdateTenantStatusInput input);
    }
}