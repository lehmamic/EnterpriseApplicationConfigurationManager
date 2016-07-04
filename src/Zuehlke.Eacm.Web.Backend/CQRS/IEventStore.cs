using System;
using System.Collections.Generic;

namespace Zuehlke.Eacm.Web.Backend.CQRS
{
    public interface IEventStore
    {
        IEnumerable<IEvent> LoadEvents<TAggregate>(Guid id) where TAggregate : IAggregateRoot, new();

        void SaveEvents<TAggregate>(Guid id, IEnumerable<IEvent> resultEvents) where TAggregate : IAggregateRoot, new();
    }
}
