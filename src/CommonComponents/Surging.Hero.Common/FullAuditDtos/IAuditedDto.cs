using Surging.Core.Domain.Entities.Auditing;

namespace Surging.Hero.Common.FullAuditDtos
{
    public interface IAuditedDto : IAudited
    {
        string CreatorUserName { get; set; }
        
        string LastModificationUserName { get; set; }
    }
}