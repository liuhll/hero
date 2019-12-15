using Surging.Core.Domain;
using Surging.Core.Domain.PagedAndSorted;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class QueryRoleInput : PagedResultRequestDto
    {
        public string SearchKey { get; set; }
    }
}
