using System;
using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.Common.FullAuditDtos
{
    public class AuditDto : IAuditedDto
    {
        public virtual DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }

        public virtual string CreatorUserName { get; set; }
        
        public DateTime? LastModificationTime { get; set; }

        public string LastModificationUserName { get; set; }

        public long? LastModifierUserId { get; set; }
    }
}