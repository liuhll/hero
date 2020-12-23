using Surging.Core.Domain.Entities.Auditing;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroup : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        
        public string Identification { get; set; }

        public string Memo { get; set; }
        public Status Status { get; set; }

        public DataPermissionType? DataPermissionType { get; set; }
    }
}