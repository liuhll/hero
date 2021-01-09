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
                var newSysconfig = new SystemConfig()
                {
                    SysName = HeroConstants.DefaultSysName,
                    NonPermissionOperationStyle = NonPermissionOperationStyle.Displayed
                };
                await _systemConfigRepository.InsertAsync(newSysconfig);
                return newSysconfig.MapTo<GetSystemConfigOutput>();
            }

            return sysConfig.MapTo<GetSystemConfigOutput>();
        }

        public async Task<string> SetSystemConfig(SetSystemConfigInput input)
        {
            var systemConfig = input.MapTo<SystemConfig>();
            systemConfig.TenantId = _session.TenantId;
            if (!input.Logo.IsNullOrEmpty())
            {
                // todo 设置logo的地址
            }

            await _systemConfigRepository.InsertOrUpdateAsync(systemConfig);
            return "设置系统配置成功";

        }
    }
}