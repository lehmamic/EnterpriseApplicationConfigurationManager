using System;
using Zuehlke.Eacm.Web.Backend.CQRS;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class ProjectAttributesChanged : IEvent
    {
        #region Implementation of IEvent
        public Guid Id { get; set; }

        public Guid SourceId { get; set; }

        public DateTime Timestamp { get; set; }

        public Guid CorrelationId { get; set; }
        #endregion

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
