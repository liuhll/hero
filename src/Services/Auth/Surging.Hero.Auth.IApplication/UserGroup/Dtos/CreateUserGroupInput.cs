namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class CreateUserGroupInput : UserGroupDtoBase
    {
        public long ParentId { get; set; }
        
        public long[] UserIds { get; set; }
    }
}
