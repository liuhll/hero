namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public abstract class WordbookItemDtoBase
    {
        public long WordbookId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Memo { get; set; }
    }
}
