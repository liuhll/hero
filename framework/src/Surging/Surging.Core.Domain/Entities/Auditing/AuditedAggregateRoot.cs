using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surging.Core.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class AuditedAggregateRoot : AuditedAggregateRoot<int>
    {
    }

    [Serializable]
    public abstract class AuditedAggregateRoot<TPrimaryKey> : CreationAuditedAggregateRoot<TPrimaryKey>, IAudited
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual long? LastModifierUserId { get; set; }
    }

    [Serializable]
    public abstract class AuditedAggregateRoot<TPrimaryKey, TUser> : AuditedAggregateRoot<TPrimaryKey>, IAudited<TUser>
        where TUser : IEntity<long>
    {
        [ForeignKey("CreatorUserId")]
        public virtual TUser CreatorUser { get; set; }

        [ForeignKey("LastModifierUserId")]
        public virtual TUser LastModifierUser { get; set; }
    }
}
