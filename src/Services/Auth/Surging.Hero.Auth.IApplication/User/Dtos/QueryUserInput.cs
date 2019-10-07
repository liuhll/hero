using Surging.Core.Domain;
using Surging.Hero.Common;
using Surging.Hero.Organization.Domain.Shared.Organizations;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class QueryUserInput : PagedResultRequestDto
    {
        public long? OrgId { get; set; }

       public Status? Status { get; set; }

        public string SearchKey { get; set; }

    }
}
