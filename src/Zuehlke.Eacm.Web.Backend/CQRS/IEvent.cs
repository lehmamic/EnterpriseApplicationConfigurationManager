using System;

namespace Zuehlke.Eacm.Web.Backend.CQRS
{
    public interface IEvent
    {
        Guid Id { get; }

        Guid SourceId { get; set; }

        DateTime Timestamp { get; set; }
        
        Guid CorrelationId { get; set; }
    }
}
