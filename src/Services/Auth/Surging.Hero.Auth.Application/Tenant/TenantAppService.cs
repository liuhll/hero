using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Cloud.ProxyGenerator;
using Surging.Cloud.Validation.DataAnnotationValidation;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Auth.Domain.Tenants;
using Surging.Hero.Auth.IApplication.Tenant;
using Surging.Hero.Auth.IApplication.Tenant.Dtos;

namespace Surging.Hero.Auth.Application.Tenant
{
    public class TenantAppService : ProxyServiceBase, ITenantAppService
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

        public async Task<IPagedResult<GetTenantPageOutput>> Search(QueryTenantInput query)
        {
            return await _tenantDomainService.Search(query);
        }

        public async Task<IEnumerable<GetTenantOutput>> List()
        {
            return await _tenantDomainService.List();
        }

        public async Task<string> Status(UpdateTenantStatusInput input)
        {
            return await _tenantDomainService.Status(input);
        }
    }
}