using Surging.Core.Domain;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class QueryRoleInput : PagedResultRequestDto
    {
        public string SearchKey { get; set; }
    }
}
