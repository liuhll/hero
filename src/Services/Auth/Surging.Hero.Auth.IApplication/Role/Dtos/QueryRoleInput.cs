using Surging.Core.CPlatform.Utilities;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class QueryRoleInput : PagedAndSingleSortedResultRequest
    {
        private string _searchKey;

        public string SearchKey
        {
            get
            {
                if (_searchKey.IsNullOrWhiteSpace()) return null;
                return _searchKey;
            }
        }

        public bool OnlySelfOrgRole { get; set; } = true;

        public Status? Status { get; set; }
    }
}