using Surging.Hero.Auth.IApplication.Role.Dtos;
using Surging.Hero.Auth.IApplication.User.Dtos;
using System;
using System.Collections.Generic;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class GetUserGroupOutput : UserGroupDtoBase
    {
        public long Id { get; set; }

        public IEnumerable<GetDisplayRoleOutput> Roles { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }

        public virtual string CreatorUserName { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        public string LastModificationUserName { get; set; }

        /// <summary>
        /// 最后修改人
        /// </summary>
        public long? LastModifierUserId { get; set; }

    }
}
