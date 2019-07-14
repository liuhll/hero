using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surging.Core.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class CreationAuditedAggregateRoot : CreationAuditedAggregateRoot<int>
    {
    }

    [Serializable]
    public abstract class CreationAuditedAggregateRoot<TPrimaryKey> : AggregateRoot<TPrimaryKey>, ICreationAudited
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual long? CreatorUserId { get; set; }

        protected CreationAuditedAggregateRoot()
        {
            CreationTime = DateTime.Now;
        }
    }

    [Serializable]
    public abstract class CreationAuditedAggregateRoot<TPrimaryKey, TUser> : CreationAuditedAggregateRoot<TPrimaryKey>, ICreationAudited<TUser>
        where TUser : IEntity<long>
    {
        [ForeignKey("CreatorUserId")]
        public virtual TUser CreatorUser { get; set; }
    }
}
