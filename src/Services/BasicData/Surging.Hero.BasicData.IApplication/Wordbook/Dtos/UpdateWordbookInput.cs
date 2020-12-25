using System.ComponentModel.DataAnnotations;
using Surging.Cloud.System.Intercept;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public class UpdateWordbookInput : WordbookDtoBase
    {
        [CacheKey(1)] public long Id { get; set; }


        [Required(ErrorMessage = "字典类型标识不允许为空")]
        [MaxLength(50, ErrorMessage = "字典类型标识长度不允许超过50")]
        public string Code { get; set; }
    }
}