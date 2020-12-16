namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class GetUserBasicOutput : UserDtoBase
    {
        public long Id { get; set; }

        public long? OrgId { get; set; }

        public string DeptName { get; set; }

        public long? PositionId { get; set; }

        public string PositionName { get; set; }

        public string UserName { get; set; }
    }
}