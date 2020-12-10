using Surging.Core.Domain;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class QueryRoleInput : PagedResultRequestDto
    {
        public string SearchKey { get; set; }
        public Status? Status { get; set; }
    }
}
