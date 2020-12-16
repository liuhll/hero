namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class CreateUserGroupInput : UserGroupDtoBase
    {
        public long[] RoleIds { get; set; }

        public long[] PermissionIds { get; set; }
    }
}