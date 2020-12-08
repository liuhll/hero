using Surging.Core.Domain.PagedAndSorted;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class QueryUserGroupInput : PagedResultRequestDto
    {
        public string SearchKey { get; set; }
    }
}
