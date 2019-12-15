using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class QueryUserInput : PagedResultRequestDto
    {
        public long? OrgId { get; set; }

       public Status? Status { get; set; }

        public string SearchKey { get; set; }

    }
}
