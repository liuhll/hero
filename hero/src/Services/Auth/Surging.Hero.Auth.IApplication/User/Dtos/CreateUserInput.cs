using Surging.Hero.Auth.Domain.Shared.User;
using Surging.Hero.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class CreateUserInput : UserDtoBase
    {
        [Required(ErrorMessage = "用户名不允许为空")]
        [RegularExpression("^[a-zA-Z0-9_-]{4,16}$", ErrorMessage = "用户名不允许为空")]
        public string UserName { get; set; }

       // [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[^]{8,16}$", ErrorMessage = "密码格式不正确")]
        public string Password { get; set; }
    }
}
