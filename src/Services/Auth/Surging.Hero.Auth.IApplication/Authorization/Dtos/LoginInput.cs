
using Surging.Hero.Common;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.Authorization.Dtos
{
    public class LoginInput
    {
        [Required(ErrorMessage = "用户名不允许为空")]
        [RegularExpression(RegExpConstants.UserName, ErrorMessage = "用户名格式不正确")]
        public string UserName { get; set; }

        [RegularExpression(RegExpConstants.Password, ErrorMessage = "密码格式不正确")]
        public string Password { get; set; }
    }
}
