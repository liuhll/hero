using Surging.Hero.BasicData.Domain.Shared.SystemConfigs;

namespace Surging.Hero.BasicData.IApplication.SystemConfig.Dtos
{
    public class GetSystemConfigOutput
    {
        public long? Id { get; set; }
        
        public string SysName { get; set; }

        public string DomainName { get; set; }

        public string Administrator { get; set; }
        
        public string LogoSite { get; set; }

        public NonPermissionOperationStyle NonPermissionOperationStyle { get; set; }
    }
}