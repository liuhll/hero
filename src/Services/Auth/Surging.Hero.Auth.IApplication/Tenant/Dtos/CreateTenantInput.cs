using System.ComponentModel.DataAnnotations;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Tenant.Dtos
{
    public class CreateTenantInput : TenantDtoBase
    {
        /// <summary>
        /// 租户对应的组织机构标识
        /// </summary>
        [Required(ErrorMessage = "组织机构标识不允许为空")]
        [MaxLength(50, ErrorMessage = "组织机构唯一标识长度不允许超过50")]
        [RegularExpression(RegExpConstants.OrgIdentificationRegex, ErrorMessage = "组织机构标识不正确,只能是字母和数字组合")]
        public string Identification { get; set; }
    }
}