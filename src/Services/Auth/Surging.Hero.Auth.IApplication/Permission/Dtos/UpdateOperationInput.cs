namespace Surging.Hero.Auth.IApplication.Permission.Dtos
{
    public class UpdateOperationInput : OperationDtoBase
    {
        public long Id { get; set; }
        public long[] ActionIds { get; set; }
    }
}
