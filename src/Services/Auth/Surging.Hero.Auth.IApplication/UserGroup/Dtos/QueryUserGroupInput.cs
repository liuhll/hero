using Surging.Cloud.CPlatform.Utilities;
using Surging.Cloud.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class QueryUserGroupInput : PagedAndSingleSortedResultRequest
    {
        
        public string SearchKey { get; set; }

        public bool OnlySelfOrgUserGroup { get; set; } = true;

        public Status? Status { get; set; }
    }
}