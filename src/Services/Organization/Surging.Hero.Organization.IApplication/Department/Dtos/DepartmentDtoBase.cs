using Surging.Hero.Common;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Organization.IApplication.Department.Dtos
{
    public abstract class DepartmentDtoBase
    {

        [Required(ErrorMessage = "部门名称不允许为空")]
        [MaxLength(50,ErrorMessage = "部门名称长度不允许超过50")]
        public string Name { get; set; }

        public string Location { get; set; }

        public long DeptTypeId { get; set; }

        [MaxLength(100, ErrorMessage = "部门简介长度不允许超过100")]
        public string BriefIntro { get; set; }

        [MaxLength(500, ErrorMessage = "部门备注不允许超过500")]
        public string Memo { get; set; }
    }
}
