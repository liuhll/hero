using Surging.Hero.Auth.Domain.Shared.Operations;
using Surging.Hero.Common;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.Permission.Dtos
{
    public abstract class OperationDtoBase
    {
     
        public string Icon { get; set; }

        [Required(ErrorMessage = "操作标识不允许为空")]
        [MaxLength(50, ErrorMessage = "操作标识长度不允许超过50")]
        public string Name { get; set; }

        [Required(ErrorMessage = "操作名称不允许为空")]
        [MaxLength(50, ErrorMessage = "操作名称长度不允许超过50")]
        public string Title { get; set; }

        public OperationMold Mold { get; set; }

        public string Memo { get; set; }

    }
}
