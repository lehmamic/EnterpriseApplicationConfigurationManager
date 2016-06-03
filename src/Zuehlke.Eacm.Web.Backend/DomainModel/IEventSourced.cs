using System;
using System.Collections.Generic;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public interface IEventSourced : IAggregateRoot
    {
        DateTime Created { get; }

        DateTime Modified { get; }

        IEnumerable<IEvent> PendingEvents { get; }
    }
}