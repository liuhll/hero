using System;
using Surging.Hero.Common;
using Surging.Hero.Common.Extensions;
using Surging.Hero.Common.FullAuditDtos;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class GetRoleOutput : RoleDtoBase, IAuditedDto
    {
        /// <summary>
        ///     主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     该角色所拥有的权限
        /// </summary>
        public long[] PermissionIds { get; set; }

        /// <summary>
        /// 如果是用户自定义数据权限,则该角色拥有的数据权限的部门
        /// </summary>
        public long[] DataPermissionOrgIds { get; set; }

        /// <summary>
        ///     状态
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        ///     状态描述
        /// </summary>
        public string StatusDesc => Status.GetDescription();

        public virtual DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }

        public virtual string CreatorUserName { get; set; }

        /// <summary>
        ///     最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        public string LastModificationUserName { get; set; }

        /// <summary>
        ///     最后修改人
        /// </summary>
        public long? LastModifierUserId { get; set; }
    }
}