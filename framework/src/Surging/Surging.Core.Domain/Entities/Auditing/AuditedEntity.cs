using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Surging.Core.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class AuditedEntity : AuditedEntity<int>, IEntity
    {
    }

    [Serializable]
    public abstract class AuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>, IAudited
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual long? LastModifierUserId { get; set; }
    }

    [Serializable]
    public abstract class AuditedEntity<TPrimaryKey, TUser> : AuditedEntity<TPrimaryKey>, IAudited<TUser>
        where TUser : IEntity<long>
    {
        [ForeignKey("CreatorUserId")]
        public virtual TUser CreatorUser { get; set; }

        [ForeignKey("LastModifierUserId")]
        public virtual TUser LastModifierUser { get; set; }
    }
}
