using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surging.Core.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class CreationAuditedEntity : CreationAuditedEntity<int>, IEntity
    {
    }

    [Serializable]
    public abstract class CreationAuditedEntity<TPrimaryKey> : Entity<TPrimaryKey>, ICreationAudited
    {
        public virtual DateTime CreationTime { get; set; }

        public virtual long? CreatorUserId { get; set; }

        protected CreationAuditedEntity()
        {
            CreationTime = DateTime.Now;
        }
    }

    [Serializable]
    public abstract class CreationAuditedEntity<TPrimaryKey, TUser> : CreationAuditedEntity<TPrimaryKey>, ICreationAudited<TUser>
        where TUser : IEntity<long>
    {
        [ForeignKey("CreatorUserId")]
        public virtual TUser CreatorUser { get; set; }
    }
}
