namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public class GetWordbookItemOutput : WordbookItemDtoBase
    {
        public long Id { get; set; }

        public string Key { get; set; }

        public string WordbookCode { get; set; }
      
    }
}
