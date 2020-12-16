using System;
using System.ComponentModel.DataAnnotations;
using Surging.Hero.Organization.Domain.Shared;

namespace Surging.Hero.Organization.IApplication.Corporation.Dtos
{
    public abstract class CorporationDtoBase
    {
        [Required(ErrorMessage = "公司名称不允许为空")]
        [MaxLength(50, ErrorMessage = "公司名称不允许超过50个字符")]
        public string Name { get; set; }

        [Required(ErrorMessage = "组织机构标识不允许为空")]
        [MaxLength(50, ErrorMessage = "组织机构唯一标识长度不允许超过50")]
        [RegularExpression(OrgAppConstants.OrgIdentificationRegex, ErrorMessage = "组织机构标识不正确,只能是字母和数字组合")]
        public string Identification { get; set; }

        public CorporationMold Mold { get; set; }

        [Required(ErrorMessage = "公司地址不允许为空")]
        [MaxLength(100, ErrorMessage = "公司地址不允许超过100个字符")]
        public string Address { get; set; }

        public string Logo { get; set; }

        public string LogoPosition { get; set; }

        public string CorporateRepresentative { get; set; }

        public DateTime RegisterDate { get; set; }


        public DateTime OpenDate { get; set; }

        public string Trade { get; set; }

        public string Memo { get; set; }
    }
}