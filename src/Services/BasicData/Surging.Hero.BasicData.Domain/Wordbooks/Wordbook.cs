using Surging.Cloud.Domain.Entities.Auditing;
using Surging.Hero.BasicData.Domain.Shared.Wordbooks;

namespace Surging.Hero.BasicData.Domain.Wordbooks
{
    public class Wordbook : FullAuditedEntity<long>, IMultiTenant
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public WordbookType Type { get; set; }

        public string Memo { get; set; }

        public bool IsSysPreset { get; set; } = false;
        
        public long? TenantId { get; set; }
    }
}