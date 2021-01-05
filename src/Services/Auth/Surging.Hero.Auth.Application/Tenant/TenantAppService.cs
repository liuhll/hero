using System.Threading.Tasks;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Cloud.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Tenants;
using Surging.Hero.Auth.IApplication.Tenant;
using Surging.Hero.Auth.IApplication.Tenant.Dtos;

namespace Surging.Hero.Auth.Application.Tenant
{
    public class TenantAppService : ITenantAppService
    {
        private readonly ITenantDomainService _tenantDomainService;

        public TenantAppService(ITenantDomainService tenantDomainService)
        {
            _tenantDomainService = tenantDomainService;
        }

        public async Task<string> Create(CreateTenantInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            return await _tenantDomainService.Create(input);
        }

        public async Task<string> Update(UpdateTenantInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            return await _tenantDomainService.Update(input);
        }

        public async Task<string> Delete(long id)
        {
            return await _tenantDomainService.Delete(id);
        }

        public async Task<IPagedResult<GetTenantOutput>> Search(QueryTenantInput query)
        {
            return await _tenantDomainService.Search(query);
        }
    }
}