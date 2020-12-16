using System.ComponentModel.DataAnnotations;
using Surging.Core.System.Intercept;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public class UpdateWordbookItemInput : WordbookItemDtoBase
    {
        [CacheKey(1)] public long Id { get; set; }

        /// <summary>
        ///     字典的key
        /// </summary>
        [Required(ErrorMessage = "字典项的key不允许为空")]
        [MaxLength(50, ErrorMessage = "字典项的值长度不允许超过50")]
        public string Key { get; set; }
    }
}