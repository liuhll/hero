using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserUserGroupRelation : AuditedEntity<long>
    {
        public long UserId { get; set; }

        public long UserGroupId { get; set; }
    }
}