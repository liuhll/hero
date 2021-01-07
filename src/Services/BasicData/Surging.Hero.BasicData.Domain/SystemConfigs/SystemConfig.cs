using Surging.Cloud.Domain.Entities.Auditing;
using Surging.Hero.BasicData.Domain.Shared.SystemConfigs;

namespace Surging.Hero.BasicData.Domain.SystemConfigs
{
    public class SystemConfig : FullAuditedEntity<long>, IMultiTenant
    {
        public string SysName { get; set; }

        public string DomainName { get; set; }

        public string Administrator { get; set; }

        public string Logo { get; set; }
        
        public string LogoPosition { get; set; }
        
        public string LogoSite { get; set; }

        public NonPermissionOperationStyle NonPermissionOperationStyle { get; set; }
        
        public long? TenantId { get; set; }
    }
}