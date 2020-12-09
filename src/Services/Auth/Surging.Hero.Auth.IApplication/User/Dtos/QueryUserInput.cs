using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class QueryUserInput : PagedAndSingleSortedResultRequest
    {
        public long? OrgId { get; set; }

        public long? PositionId { get; set; }

       public Status? Status { get; set; }

        public string SearchKey { get; set; }

    }
}
