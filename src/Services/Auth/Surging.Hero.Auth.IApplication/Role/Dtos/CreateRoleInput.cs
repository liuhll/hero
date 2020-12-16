namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class CreateRoleInput : RoleDtoBase
    {
        /// <summary>
        ///     选定的权限Ids
        /// </summary>
        public long[] PermissionIds { get; set; }
    }
}