using DapperExtensions.Mapper;
using Surging.Core.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Test.Server.DapperTest.Users.ClassMapper
{
    public class UserInfoClassMapper : ClassMapper<UserInfo>
    {
        public UserInfoClassMapper()
        {
            Map(p => p.Id).Key(KeyType.Identity);

            Map(p => ((ICreationAudited)p).CreationTime).Column("CreateTime");
            Map(p => ((ICreationAudited)p).CreatorUserId).Column("CreateBy");
            Map(p => ((IModificationAudited)p).LastModificationTime).Column("UpdateTime");
            Map(p => ((IModificationAudited)p).LastModifierUserId).Column("UpdateBy");
            Map(p => ((IDeletionAudited)p).DeleterUserId).Column("DeleteBy");
            Map(p => ((IDeletionAudited)p).DeletionTime).Column("DeleteTime");
            Map(p => ((IDeletionAudited)p).IsDeleted).Column("IsDeleted");
            AutoMap();
        }
    }
}
