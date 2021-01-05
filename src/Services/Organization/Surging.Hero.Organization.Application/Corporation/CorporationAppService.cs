using System.Threading.Tasks;
using Surging.Cloud.ProxyGenerator;
using Surging.Cloud.Validation.DataAnnotationValidation;
using Surging.Hero.Organization.Domain.Organizations;
using Surging.Hero.Organization.IApplication.Corporation;
using Surging.Hero.Organization.IApplication.Corporation.Dtos;

namespace Surging.Hero.Organization.Application.Corporation
{
    public class CorporationAppService : ProxyServiceBase, ICorporationAppService
    {
        private readonly ICorporationDomainService _corporationDomainService;

        public CorporationAppService(ICorporationDomainService corporationDomainService)
        {
            _corporationDomainService = corporationDomainService;
        }

        public async Task<CreateCorporationOutput> Create(CreateCorporationInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            return await _corporationDomainService.CreateCorporation(input);
        }

        public async Task<CreateCorporationOutput> CreateByTenant(CreateCorporationByTenantInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            return await _corporationDomainService.CreateByTenant(input);
        }

        public async Task<string> DeleteByOrgId(long orgId)
        {
            await _corporationDomainService.DeleteCorporation(orgId);
            return "删除公司信息成功";
        }

        public async Task<GetCorporationOutput> GetByOrgId(long orgId)
        {
            return await _corporationDomainService.GetCorporation(orgId);
        }

        public async Task<UpdateCorporationOutput> Update(UpdateCorporationInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            return await _corporationDomainService.UpdateCorporation(input);
        }
    }
}