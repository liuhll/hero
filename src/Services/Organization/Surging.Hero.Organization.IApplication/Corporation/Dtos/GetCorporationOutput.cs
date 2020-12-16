namespace Surging.Hero.Organization.IApplication.Corporation.Dtos
{
    public class GetCorporationOutput : CorporationDtoBase
    {
        public long Id { get; set; }

        public string Code { get; set; }
    }
}