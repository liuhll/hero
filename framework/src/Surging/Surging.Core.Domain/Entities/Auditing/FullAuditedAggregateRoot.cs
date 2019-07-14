using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surging.Core.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class FullAuditedAggregateRoot : FullAuditedAggregateRoot<int>
    {
    }

    [Serializable]
    public abstract class FullAuditedAggregateRoot<TPrimaryKey> : AuditedAggregateRoot<TPrimaryKey>, IFullAudited
    {
        public virtual int IsDeleted { get; set; }

        public virtual long? DeleterUserId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }
    }

    [Serializable]
    public abstract class FullAuditedAggregateRoot<TPrimaryKey, TUser> : AuditedAggregateRoot<TPrimaryKey, TUser>, IFullAudited<TUser>
        where TUser : IEntity<long>
    {
        public virtual int IsDeleted { get; set; }

        [ForeignKey("DeleterUserId")]
        public virtual TUser DeleterUser { get; set; }

        public virtual long? DeleterUserId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }
    }
}
