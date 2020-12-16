using Surging.Core.Domain.PagedAndSorted;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public class GetWordbookItemsInput : PagedResultRequestDto
    {
        public long? WordbookId { get; set; }

        public string Code { get; set; }
    }
}