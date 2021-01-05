namespace Surging.Hero.Auth.IApplication.Tenant.Dtos
{
    public class UpdateTenantInput : TenantDtoBase
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public long Id { get; set; }
    }
}