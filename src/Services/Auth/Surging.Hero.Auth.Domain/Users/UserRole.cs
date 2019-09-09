using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.Users
{
    public class UserRole : AuditedEntity<long>
    {
        public long UserId { get; set; }

        public long RoleId { get; set; }
    }
}
