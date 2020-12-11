using Surging.Hero.Common.FullAuditDtos;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public abstract class WordbookItemDtoBase : FullAuditDto
    {     
        [Required(ErrorMessage = "字典项值不允许为空")]
        [MaxLength(50, ErrorMessage = "字典项值长度不允许超过50")]
        public string Value { get; set; }

        public string Memo { get; set; }

        public int Sort { get; set; }
    }
}
