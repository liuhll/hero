using System;

namespace Surging.Hero.Common.FullAuditDtos
{
    public class CreateDto
    {
        public virtual DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }

        public virtual string CreatorUserName { get; set; }
    }
}