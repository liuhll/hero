using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.System.Intercept;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public class GetWordbookItemsInput : PagedResultRequestDto
    {
        public long? WordbookId { get; set; }

        public string Code { get; set; }

    }
}
