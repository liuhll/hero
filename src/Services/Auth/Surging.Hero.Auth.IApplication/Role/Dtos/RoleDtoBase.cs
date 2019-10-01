using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public abstract class RoleDtoBase 
    {

        [Required(ErrorMessage = "角色名称不允许为空")]
        [MaxLength(50,ErrorMessage = "角色名称不允许超过50个字符")]
        public string Name { get; set; }

        [MaxLength(200, ErrorMessage = "备注长度不允许超过200个字符")]
        public string Memo { get; set; }
    }
}
