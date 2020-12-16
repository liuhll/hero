namespace Surging.Hero.Organization.IApplication.Position.Dtos
{
    public class GetPositionOutput : PositionDtoBase
    {
        public long DeptId { get; set; }
        public long Id { get; set; }

        public string FunctionName { get; set; }

        public string PositionLevelName { get; set; }
    }
}