using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroup : FullAuditedEntity<long>
    {
        public string Name { get; set; }

        public string Memo { get; set; }

        public Common.Status Status { get; set; }
    }
}
