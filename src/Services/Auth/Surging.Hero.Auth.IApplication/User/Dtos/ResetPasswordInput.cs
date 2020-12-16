using System.ComponentModel.DataAnnotations;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class ResetPasswordInput
    {
        public long Id { get; set; }

        [RegularExpression(RegExpConstants.Password, ErrorMessage = "密码格式不正确")]
        public string NewPassword { get; set; }
    }
}