using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Tenant.Dtos
{
    public class GetTenantOutput : TenantDtoBase
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 租户状态
        /// </summary>
        public Status Status { get; set; }
    }
}