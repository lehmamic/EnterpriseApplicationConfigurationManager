using System;
using System.Collections.Generic;

namespace Zuehlke.Eacm.Web.Backend.CQRS
{
    public interface IEventSourced : IAggregateRoot
    {
        DateTime Created { get; }

        DateTime? Modified { get; }

        IEnumerable<IEvent> PendingEvents { get; }
    }
}