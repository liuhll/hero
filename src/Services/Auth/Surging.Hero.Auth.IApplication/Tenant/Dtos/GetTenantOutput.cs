using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Tenant.Dtos
{
    public class GetTenantOutput : TenantDtoBase
    {
        public long Id { get; set; }

        public Status Status { get; set; }
    }
}