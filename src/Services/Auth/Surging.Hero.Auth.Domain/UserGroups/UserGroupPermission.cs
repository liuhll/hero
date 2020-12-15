using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Auth.Domain.UserGroups
{
    public class UserGroupPermission : AuditedEntity<long>
    {
        public long UserGroupId { get; set; }
        
        public long PermissionId { get; set; }
    }
}