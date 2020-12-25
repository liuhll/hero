using Surging.Core.CPlatform.Utilities;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class QueryRoleInput : PagedAndSingleSortedResultRequest
    {

        public string SearchKey { get; set; }
        
        public bool OnlySelfOrgRole { get; set; } = true;

        public Status? Status { get; set; }
    }
}