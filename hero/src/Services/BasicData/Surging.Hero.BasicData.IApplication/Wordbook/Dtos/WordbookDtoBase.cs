using Surging.Hero.Common;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public abstract class WordbookDtoBase
    {
        [Required(ErrorMessage = "字典编码不允许为空")]
        [RegularExpression(RegExpConstants.WordbookCode, ErrorMessage = "字典类型编码格式不正确")]
        public string Code { get; set; }

        [Required(ErrorMessage = "字典类型名称不允许为空")]
        [MaxLength(50,ErrorMessage = "字典类型名称长度不允许超过50")]
        public string Name { get; set; }
       
        [MaxLength(100, ErrorMessage = "字典类型备注长度不允许超过100")]
        public string Memo { get; set; }
    }
}
