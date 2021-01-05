namespace Surging.Hero.Organization.IApplication.Corporation.Dtos
{
    public class CreateCorporationByTenantInput : CreateCorporationInput
    {
        public long TenantId { get; set; }
    }
}