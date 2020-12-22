using System.ComponentModel.DataAnnotations;
using Surging.Hero.Auth.Domain.Shared;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public abstract class UserGroupDtoBase
    {
        /// <summary>
        /// 用户组名称
        /// </summary>
        [Required(ErrorMessage = "用户组名称不允许为空")]
        [MaxLength(50, ErrorMessage = "用户组名称长度不允许超过50个字符")]
        public string Name { get; set; }
        
        /// <summary>
        /// 数据权限
        /// </summary>
        public DataPermissionType DataPermissionType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100, ErrorMessage = "备注不允许超过100个字符")]
        public string Memo { get; set; }
    }
}