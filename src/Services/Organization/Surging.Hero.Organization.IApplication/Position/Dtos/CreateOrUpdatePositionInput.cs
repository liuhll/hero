namespace Surging.Hero.Organization.IApplication.Position.Dtos
{
    public class CreateOrUpdatePositionInput : PositionDtoBase
    {
        //public long DeptId { get; set; }
        /// <summary>
        ///     职务id
        /// </summary>
        public long? Id { get; set; }
    }
}