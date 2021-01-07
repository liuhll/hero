using System.Threading.Tasks;
using Surging.Cloud.ProxyGenerator;
using Surging.Hero.BasicData.Domain.SystemConfigs;
using Surging.Hero.BasicData.IApplication.SystemConfig;
using Surging.Hero.BasicData.IApplication.SystemConfig.Dtos;

namespace Surging.Hero.BasicData.Application.SystemConfig
{
    public class SystemConfigAppService : ProxyServiceBase, ISystemConfigAppService
    {
        private readonly ISystemConfigDomainService _systemConfigDomainService;

        public SystemConfigAppService(ISystemConfigDomainService systemConfigDomainService)
        {
            _systemConfigDomainService = systemConfigDomainService;
        }


        public async Task<GetSystemConfigOutput> GetSystemConfig()
        {
            return await _systemConfigDomainService.GetSystemConfig();
        }
    }
}