using Surging.Hero.Common;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Organization.IApplication.Position.Dtos
{
    public abstract class PositionDtoBase
    {
        public long DeptId { get; set; }

        [Required(ErrorMessage = "职位编码不允许为空")]
        [RegularExpression(RegExpConstants.PositionCode, ErrorMessage = "职位编码格式不正确")]
        public string Code { get; set; }

        [Required(ErrorMessage = "职位名称不允许为空")]
        [MaxLength(50, ErrorMessage = "职位名称长度不允许超过50")]
        public string Name { get; set; }

        public long FunctionId { get; set; }

        public long PositionLevelId { get; set; }

        [MaxLength(100, ErrorMessage = "职位简介长度不允许超过100")]
        public string BriefIntro { get; set; }

        [MaxLength(500, ErrorMessage = "职位备注长度不允许超过500")]
        public string Memo { get; set; }

        [Required(ErrorMessage = "岗位职责不允许为空")]
        [MaxLength(200, ErrorMessage = "岗位职责长度不允许超过200")]
        public string PostResponsibility { get; set; }

        public bool IsLeadershipPost { get; set; }
    }
}
