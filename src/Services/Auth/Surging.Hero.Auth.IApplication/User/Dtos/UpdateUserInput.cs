namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class UpdateUserInput : UserDtoBase
    {
        public long Id { get; set; }

        public long? OrgId { get; set; }

        public long? PositionId { get; set; }

        /// <summary>
        /// 分配的角色
        /// </summary>
        public long[] RoleIds { get; set; }

        /// <summary>
        /// 分配的用户组
        /// </summary>
        public long[] UserGroupIds { get; set; }
    }
}
