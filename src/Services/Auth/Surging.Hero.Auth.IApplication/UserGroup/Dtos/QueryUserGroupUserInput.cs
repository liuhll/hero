using Surging.Core.Domain.PagedAndSorted;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class QueryUserGroupUserInput : PagedResultRequestDto
    {
        public long UserGroupId { get; set; }

        public string SearchKey { get; set; }
    }
}
