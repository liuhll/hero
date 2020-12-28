using System.ComponentModel.DataAnnotations;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Common;
using Surging.Hero.Common.Extensions;

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
        ///   唯一标识,系统中不允许重复
        /// </summary>
        [Required(ErrorMessage = "角色名称不允许为空")]
        [MaxLength(50, ErrorMessage = "角色名称不允许超过50个字符")]
        [RegularExpression(RegExpConstants.NormalIdentificationCode, ErrorMessage = "角色唯一标识不正确")]
        public string Identification { get; set; }

        /// <summary>
        ///    数据权限
        /// </summary>
        public DataPermissionType DataPermissionType { get; set; }

        /// <summary>
        /// 数据权限描述
        /// </summary>
        public string DataPermissionTypeDes
        {
            get { return this.DataPermissionType.GetDescription(); }
        }
        
        /// <summary>
        /// 角色是否可以被分配给所有用户
        /// </summary>
        public bool IsAllOrg { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public virtual long[] OrgIds { get; set; }
        
        /// <summary>
        ///    备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "备注长度不允许超过200个字符")]
        public string Memo { get; set; }

    }
}