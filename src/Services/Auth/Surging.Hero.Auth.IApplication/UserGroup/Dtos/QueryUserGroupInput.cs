using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class QueryUserGroupInput : PagedResultRequestDto
    {
        public string SearchKey { get; set; }

        public Status? Status { get; set; }
    }
}
