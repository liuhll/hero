using Microsoft.Extensions.Configuration;
using Surging.Cloud.CPlatform;
using Surging.Cloud.CPlatform.Exceptions;

namespace Surging.Hero.Auth.Domain.Tenants
{
    public class TenantConfigProvider : ITenantConfigProvider
    {
        public TenantConfig Get()
        {
            if (AppConfig.Configuration == null)
            {
                throw new BusinessException("获取配置文件失败");
            }

            var tenantConfigSection = AppConfig.GetSection("Tenant");
            if (tenantConfigSection == null)
            {
                throw new BusinessException("不存在默认租户账号配置项节点");
            }
            return tenantConfigSection.Get<TenantConfig>();
        }
    }
}