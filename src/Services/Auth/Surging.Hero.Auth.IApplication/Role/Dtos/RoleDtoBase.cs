using System.ComponentModel.DataAnnotations;
using Surging.Hero.Auth.Domain.Shared;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public abstract class RoleDtoBase
    {
        /// <summary>
        ///    角色名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不允许为空")]
        [MaxLength(50, ErrorMessage = "角色名称不允许超过50个字符")]
        public string Name { get; set; }

        /// <summary>
        ///    数据权限
        /// </summary>
        public DataPermissionType DataPermissionType { get; set; }

        /// <summary>
        ///    备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "备注长度不允许超过200个字符")]
        public string Memo { get; set; }
    }
}