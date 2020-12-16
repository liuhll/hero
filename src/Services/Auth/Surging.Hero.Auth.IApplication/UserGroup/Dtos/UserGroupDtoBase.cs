using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public abstract class UserGroupDtoBase
    {
        [Required(ErrorMessage = "用户组名称不允许为空")]
        [MaxLength(50, ErrorMessage = "用户组名称长度不允许超过50个字符")]
        public string Name { get; set; }

        [MaxLength(100, ErrorMessage = "备注不允许超过100个字符")]
        public string Memo { get; set; }
    }
}