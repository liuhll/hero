using System;

namespace Surging.Hero.Common.FullAuditDtos
{
    public class FullAuditDto : CreateDto
    {
        public DateTime? LastModificationTime { get; set; }

        public string LastModificationUserName { get; set; }

        public long? LastModifierUserId { get; set; }
    }
}