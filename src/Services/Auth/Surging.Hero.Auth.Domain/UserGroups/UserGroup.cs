using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroup : FullAuditedEntity<long>
    {
        public long ParentId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }

        public string Memo { get; set; }

        public Common.Status Status { get; set; }
    }
}
