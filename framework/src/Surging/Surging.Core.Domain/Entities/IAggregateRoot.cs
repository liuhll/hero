using Surging.Core.CPlatform.EventBus.Events;
using System.Collections.Generic;

namespace Surging.Core.Domain.Entities
{
    public interface IAggregateRoot : IAggregateRoot<int>, IEntity
    {
    }

    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        ICollection<IntegrationEvent> DomainEvents { get; }
    }
}
