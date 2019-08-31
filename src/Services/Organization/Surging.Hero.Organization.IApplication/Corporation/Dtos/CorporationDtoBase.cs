using Surging.Hero.Organization.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surging.Hero.Organization.IApplication.Corporation.Dtos
{
    public abstract class CorporationDtoBase
    {
        [Required(ErrorMessage = "公司名称不允许为空")]
        [MaxLength(50,ErrorMessage = "公司名称不允许超过50个字符")]
        public string Name { get; set; }

       
        public CorporationType Type { get; set; }

        public CorporationMold Mold { get; set; }

        [Required(ErrorMessage = "公司地址不允许为空")]
        [MaxLength(100, ErrorMessage = "公司地址不允许超过100个字符")]
        public string Address { get; set; }

        public string Logo { get; set; }

        public string LogoPosition { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime OpenDate { get; set; }

        public string Trade { get; set; }

        public string Memo { get; set; }
    }
}
