using Surging.Cloud.Domain.Entities.Auditing;

namespace Surging.Hero.BasicData.Domain.Wordbooks
{
    public class WordbookItem : FullAuditedEntity<long>
    {
        public long WordbookId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Memo { get; set; }

        public int Sort { get; set; }
    }
}