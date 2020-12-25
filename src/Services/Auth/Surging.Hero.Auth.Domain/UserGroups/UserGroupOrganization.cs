using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupOrganization : AuditedEntity<long>
    {
        public long UserGroupId { get; set; }
        
        public long OrgId { get; set; }
    }
}