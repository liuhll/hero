using Surging.Core.Domain.Entities.Auditing;


namespace Surging.Hero.BasicData.Domain.Wordbooks
{
    public class WordbookItem : FullAuditedEntity<long>
    {
        public long WordbookItemId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Memo { get; set; }
    }
}
