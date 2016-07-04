using System;
using Zuehlke.Eacm.Web.Backend.CQRS;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public abstract class EventBase : IEvent
    {
        #region Implementation of IEvent
        public Guid Id { get; set; }

        public Guid SourceId { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid CorrelationId { get; set; }
        #endregion
    }
}
