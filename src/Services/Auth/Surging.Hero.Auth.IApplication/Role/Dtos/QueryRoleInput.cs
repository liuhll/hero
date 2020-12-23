using Surging.Core.CPlatform.Utilities;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class QueryRoleInput : PagedResultRequestDto
    {
        private string _searchKey;

        public bool IncludeSelfCreate { get; set; } = false;

        public string SearchKey
        {
            get
            {
                if (_searchKey.IsNullOrWhiteSpace()) return null;
                return _searchKey;
            }
            set => _searchKey = value;
        }

        public Status? Status { get; set; }
    }
}