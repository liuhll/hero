using System.Threading.Tasks;
using Surging.Core.AutoMapper;
using Surging.Core.ProxyGenerator;
using Surging.Core.Validation.DataAnnotationValidation;
using Surging.Hero.Common.Dtos;
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

        public async Task<string> Delete(long id)
        {
            await _corporationDomainService.DeleteCorporation(id);
            return "删除公司信息成功";
        }

        public async Task<GetCorporationOutput> Get(long id)
        {
            var corporation = await _corporationDomainService.GetCorporation(id);
            return corporation.MapTo<GetCorporationOutput>();
        }

        public async Task<string> Update(UpdateCorporationInput input)
        {
            input.CheckDataAnnotations().CheckValidResult();
            await _corporationDomainService.UpdateCorporation(input);
            return "更新公司信息成功";
        }
    }
}
