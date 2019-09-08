using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupRole : AuditedEntity<long>
    {
        public long RoleId { get; set; }

        public long UserGroupId { get; set; }
    }
}
