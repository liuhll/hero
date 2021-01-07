using System.Threading.Tasks;
using Surging.Cloud.AutoMapper;
using Surging.Cloud.CPlatform.Runtime.Session;
using Surging.Cloud.Dapper.Manager;
using Surging.Cloud.Dapper.Repositories;
using Surging.Hero.BasicData.Domain.Shared.SystemConfigs;
using Surging.Hero.BasicData.IApplication.SystemConfig.Dtos;
using Surging.Hero.Common;

namespace Surging.Hero.BasicData.Domain.SystemConfigs
{
    public class SystemConfigDomainService : ManagerBase, ISystemConfigDomainService
    {
        private readonly IDapperRepository<SystemConfig, long> _systemConfigDapperRepository;
        private readonly ISurgingSession _session;

        public SystemConfigDomainService(IDapperRepository<SystemConfig, long> systemConfigDapperRepository)
        {
            _systemConfigDapperRepository = systemConfigDapperRepository;
            _session = NullSurgingSession.Instance;
        }

        public async Task<GetSystemConfigOutput> GetSystemConfig()
        {
            var sysConfig = await _systemConfigDapperRepository.FirstOrDefaultAsync(p=> p.TenantId == _session.TenantId);
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
    }
}