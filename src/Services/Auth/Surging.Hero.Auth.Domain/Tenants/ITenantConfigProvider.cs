using Surging.Cloud.CPlatform.Ioc;

namespace Surging.Hero.Auth.Domain.Tenants
{
    public interface ITenantConfigProvider: ITransientDependency
    {
        TenantConfig Get();
    }
}