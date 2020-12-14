using Surging.Hero.Common;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Organization.IApplication.Department.Dtos
{
    public abstract class DepartmentDtoBase
    {

        [Required(ErrorMessage = "部门名称不允许为空")]
        [MaxLength(50,ErrorMessage = "部门名称长度不允许超过50")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "组织机构标识不允许为空")]
        [MaxLength(50,ErrorMessage = "组织机构唯一标识长度不允许超过50")]
        [RegularExpression(OrgAppConstants.OrgIdentificationRegex,ErrorMessage = "组织机构标识不正确,只能是字母和数字组合")]
        public string Identification { get; set; }

        public string Location { get; set; }

        public string DeptTypeKey { get; set; }

        [MaxLength(100, ErrorMessage = "部门简介长度不允许超过100")]
        public string BriefIntro { get; set; }

    }
}
