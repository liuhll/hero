using Surging.Core.Domain.PagedAndSorted;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class QueryUserGroupUserInput : PagedAndSingleSortedResultRequest
    {
        public long UserGroupId { get; set; }

        public long? OrgId { get; set; }

        public long? PositionId { get; set; }

        public string SearchKey { get; set; }
    }
}
