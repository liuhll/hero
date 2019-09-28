using Surging.Hero.Auth.Domain.Shared.Users;
using Surging.Hero.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public abstract class UserDtoBase
    {

        [Required(ErrorMessage = "员工名称不允许为空")]
        public string ChineseName { get; set; }

        [EmailAddress(ErrorMessage = "电子邮箱格式不正确")]
        public string Email { get; set; }

        [RegularExpression(RegExpConstants.Phone, ErrorMessage = "手机号码格式不正确")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "性别不允许为空")]
        public Gender? Gender { get; set; }

        public DateTime? Birth { get; set; }

        public string NativePlace { get; set; }

        public string Address { get; set; }

        public string Folk { get; set; }

        public PoliticalStatus? PoliticalStatus { get; set; }

        public string GraduateInstitutions { get; set; }

        public string Education { get; set; }

        public string Major { get; set; }

        public string Resume { get; set; }

        public string Memo { get; set; }

        public Status Status { get; set; }
    }
}
