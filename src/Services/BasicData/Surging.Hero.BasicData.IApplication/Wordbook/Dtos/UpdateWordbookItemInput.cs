using Surging.Core.System.Intercept;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public class UpdateWordbookItemInput : WordbookItemDtoBase
    {
        [CacheKey(1)]
        public long Id { get; set; }
    }
}
