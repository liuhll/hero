using Surging.Core.System.Intercept;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public class UpdateWordbookInput : WordbookDtoBase
    {
        [CacheKey(1)]
        public long Id { get; set; }
    }
}
