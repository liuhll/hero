using Surging.Core.Domain;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public class QueryWordbookInput : PagedResultRequestDto
    {
        public string SearchKey { get; set; }
    }
}
