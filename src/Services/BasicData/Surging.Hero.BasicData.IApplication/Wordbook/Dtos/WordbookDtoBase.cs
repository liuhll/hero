using System.ComponentModel.DataAnnotations;
using Surging.Hero.BasicData.Domain.Shared.Wordbooks;
using Surging.Hero.Common.FullAuditDtos;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public abstract class WordbookDtoBase : FullAuditDto
    {
        [Required(ErrorMessage = "字典类型名称不允许为空")]
        [MaxLength(50, ErrorMessage = "字典类型名称长度不允许超过50")]
        public string Name { get; set; }

        public WordbookType Type { get; set; }

        [MaxLength(100, ErrorMessage = "字典类型备注长度不允许超过100")]
        public string Memo { get; set; }
    }
}