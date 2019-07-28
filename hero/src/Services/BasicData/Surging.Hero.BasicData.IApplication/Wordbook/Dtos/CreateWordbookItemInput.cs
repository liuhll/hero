using Surging.Hero.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surging.Hero.BasicData.IApplication.Wordbook.Dtos
{
    public class CreateWordbookItemInput : WordbookItemDtoBase
    {
        [Required(ErrorMessage = "字典项编码不允许为空")]
        [RegularExpression(RegExpConstants.WordbookCode, ErrorMessage = "字典项编码格式不正确")]
        public string Key { get; set; }
    }
}
