namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class SetRolePermissionInput
    {
        public long RoleId { get; set; }

        public long[] PermissionIds { get; set; }
    }
}