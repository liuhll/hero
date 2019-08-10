using System.Threading.Tasks;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
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
        public async Task<string> Create(CreateCorporationInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _corporationDomainService.CreateCorporation(input);
            return "新增公司信息成功";
        }

        public async Task<string> Update(UpdateCorporationInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _corporationDomainService.UpdateCorporation(input);
            return "更新公司信息成功";
        }
    }
}
