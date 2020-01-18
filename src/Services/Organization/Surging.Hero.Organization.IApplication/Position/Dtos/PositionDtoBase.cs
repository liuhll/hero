using Surging.Hero.Common;
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Organization.IApplication.Position.Dtos
{
    public abstract class PositionDtoBase
    {

        [Required(ErrorMessage = "职位名称不允许为空")]
        [MaxLength(50, ErrorMessage = "职位名称长度不允许超过50")]
        public string Name { get; set; }

        public long FunctionId { get; set; }

        public long PositionLevelId { get; set; }

        [Required(ErrorMessage = "岗位职责不允许为空")]
        [MaxLength(200, ErrorMessage = "岗位职责长度不允许超过200")]
        public string PostResponsibility { get; set; }

        public bool IsLeadershipPost { get; set; }

        public bool IsLeadingOfficial { get; set; }
    }
}
