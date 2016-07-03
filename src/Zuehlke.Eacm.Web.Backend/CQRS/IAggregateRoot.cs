using System;
using System.Collections.Generic;

namespace Zuehlke.Eacm.Web.Backend.CQRS
{
    public interface IAggregateRoot
    {
        Guid Id { get; set; }
        
        DateTime Created { get; }

        DateTime? Modified { get; }

        void HandleEvents(IEnumerable<IEvent> events);
    }
}