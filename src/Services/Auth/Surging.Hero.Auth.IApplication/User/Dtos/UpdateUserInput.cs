namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class UpdateUserInput : UserDtoBase
    {
        public long Id { get; set; }

        public long DeptId { get; set; }

        public long PositionId { get; set; }

        public long[] RoleIds { get; set; }
    }
}
