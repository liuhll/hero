namespace Surging.Hero.Auth.IApplication.Permission.Dtos
{
    public class CreateMenuInput : MenuDtoBase
    {
        public long ParentPermissionId { get; set; }
    }
}