using System.Threading.Tasks;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.CPlatform.Runtime.Session;
using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.Dapper.Manager;
using Surging.Cloud.Dapper.Repositories;
using Surging.Hero.BasicData.Domain.Shared.SystemConfigs;
using Surging.Hero.BasicData.IApplication.SystemConfig.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.BasicData.Domain.SystemConfigs
{
    public class SystemConfigDomainService : ManagerBase, ISystemConfigDomainService
    {
        private readonly IDapperRepository<SystemConfig, long> _systemConfigRepository;
        private readonly ISurgingSession _session;

        public SystemConfigDomainService(IDapperRepository<SystemConfig, long> systemConfigRepository)
        {
            _systemConfigRepository = systemConfigRepository;
            _session = NullSurgingSession.Instance;
        }

        public async Task<GetSystemConfigOutput> GetSystemConfig()
        {
            var sysConfig = await _systemConfigRepository.FirstOrDefaultAsync(p=> p.TenantId == _session.TenantId);
            if (sysConfig == null)
            {
                return new GetSystemConfigOutput()
                {
                    SysName = HeroConstants.DefaultSysName,
                    NonPermissionOperationStyle = NonPermissionOperationStyle.Disabled
                };
            }

            return sysConfig.MapTo<GetSystemConfigOutput>();
        }

        public async Task<string> SetSystemConfig(SetSystemConfigInput input)
        {
            SystemConfig systemConfig;
            if (input.Id.HasValue && input.Id > 0)
            {
                systemConfig = await _systemConfigRepository.GetAsync(input.Id.Value);
                systemConfig = input.MapTo(systemConfig);
            }
            else
            {
                systemConfig = input.MapTo<SystemConfig>();
            }

            if (!input.Logo.IsNullOrEmpty())
            {
                // todo 设置logo的地址
            }

            await _systemConfigRepository.InsertOrUpdateAsync(systemConfig);
            return "设置系统配置成功";

        }
    }
}