using Surging.Core.CPlatform.EventBus.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Surging.Core.Domain.Entities
{
    public class AggregateRoot : AggregateRoot<int>, IAggregateRoot
    {
    }

    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
        [NotMapped]
        public virtual ICollection<IntegrationEvent> DomainEvents { get; }

        public AggregateRoot()
        {
            DomainEvents = new Collection<IntegrationEvent>();
        }
    }
}
