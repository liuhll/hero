namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class AllocationUserIdsInput
    {
        public long UserGroupId { get; set; }
        public long[] UserIds { get; set; }
    }
}