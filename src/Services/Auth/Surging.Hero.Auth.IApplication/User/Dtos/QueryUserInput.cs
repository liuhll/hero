using System.Collections.Generic;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class QueryUserInput : PagedAndSingleSortedResultRequest
    {
        public long? OrgId { get; set; }

        public long? UserGroupId { get; set; }

        public long? PositionId { get; set; }

        public Status? Status { get; set; }

        public string SearchKey { get; set; }

        public UserIdDto UserIds { get; set; }
    }


    public class UserIdDto
    {
        public UserIdDto()
        {
            Ids = new List<long>();
        }

        public bool Include { get; set; }
        public IEnumerable<long> Ids { get; set; }
    }
}