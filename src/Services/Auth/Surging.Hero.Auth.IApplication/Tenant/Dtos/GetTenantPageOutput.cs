using System;
using Surging.Hero.Common.FullAuditDtos;

namespace Surging.Hero.Auth.IApplication.Tenant.Dtos
{
    public class GetTenantPageOutput : GetTenantOutput, IAuditedDto
    {
        public DateTime CreationTime { get; set; }
        
        public long? CreatorUserId { get; set; }
        
        public DateTime? LastModificationTime { get; set; }
        
        public long? LastModifierUserId { get; set; }
        public string CreatorUserName { get; set; }
        public string LastModificationUserName { get; set; }
    }
}