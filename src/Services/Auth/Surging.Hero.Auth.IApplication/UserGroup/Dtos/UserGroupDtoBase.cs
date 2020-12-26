using System.ComponentModel.DataAnnotations;
using Surging.Hero.Auth.Domain.Shared;
using Surging.Hero.Common;
using Surging.Hero.Common.Extensions;

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
        ///   唯一标识,系统中不允许重复
        /// </summary>
        [Required(ErrorMessage = "用户组名称不允许为空")]
        [MaxLength(50, ErrorMessage = "用户组不允许超过50个字符")]
        [RegularExpression(RegExpConstants.NormalIdentificationCode, ErrorMessage = "用户组唯一标识不正确")]
        public string Identification { get; set; }
        
        public virtual long[] OrgIds { get; set; }
        
        public bool IsAllOrg { get; set; }
        
        /// <summary>+
        /// 数据权限
        /// </summary>
        public DataPermissionType? DataPermissionType { get; set; }

        /// <summary>
        ///  数据权限描述
        /// </summary>
        public string DataPermissionTypeDesc
        {
            get {
                if (DataPermissionType.HasValue)
                {
                    return this.DataPermissionType.Value.GetDescription();
                }

                return null;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100, ErrorMessage = "备注不允许超过100个字符")]
        public string Memo { get; set; }
    }
}