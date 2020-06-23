using Surging.Hero.Common;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.Permission.Dtos
{
    public abstract class MenuDtoBase
    {

        [Required(ErrorMessage = "菜单标识不允许为空")]
        [MaxLength(50,ErrorMessage = "菜单标识长度不允许超过50")]
        public string Name { get; set; }

        [Required(ErrorMessage = "操作名称不允许为空")]
        [MaxLength(50, ErrorMessage = "操作名称长度不允许超过50")]
        public string Title { get; set; }

        public string Path { get; set; }

        public string Icon { get; set; }

        //[Required(ErrorMessage = "菜单组件名称不允许空")]
        //[MaxLength(50, ErrorMessage = "菜单组件名称不允许超过50")]
        public string Component { get; set; }

        public int Sort { get; set; }

        public string Memo { get; set; }
    }
}
