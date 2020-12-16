using Surging.Core.CPlatform.Utilities;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class QueryUserGroupInput : PagedResultRequestDto
    {
        private string _searchKey;

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