using Surging.Hero.Auth.Domain.Shared.User;
using Surging.Hero.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public abstract class UserDtoBase
    {

        [Required(ErrorMessage = "员工名称不允许为空")]
        public string ChineseName { get; set; }

        [EmailAddress(ErrorMessage = "电子邮箱格式不正确")]
        public string Email { get; set; }

        [RegularExpression("^[1](([3][0-9])|([4][5-9])|([5][0-3,5-9])|([6][5,6])|([7][0-8])|([8][0-9])|([9][1,8,9]))[0-9]{8}$", ErrorMessage = "手机号码格式不正确")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "性别不允许为空")]
        public Gender Gender { get; set; }

        public DateTime Birth { get; set; }

        public string NativePlace { get; set; }

        public string Address { get; set; }

        public string Folk { get; set; }

        public PoliticalStatus PoliticalStatus { get; set; }

        public string GraduateInstitutions { get; set; }

        public string Education { get; set; }

        public string Major { get; set; }

        public string Resume { get; set; }

        public string Memo { get; set; }

        public Status Status { get; set; }
    }
}
