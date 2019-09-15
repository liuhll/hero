namespace Surging.Hero.Auth.IApplication.Permission.Dtos
{
    public class CreateOperationInput : OperationDtoBase
    {
        public long[] ActionIds { get; set; }
    }
}
