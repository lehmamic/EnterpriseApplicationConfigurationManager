using System;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public abstract class ModelNode
    {
        protected ModelNode(IEventAggregator eventAggregator, Guid id)
        {
            this.EventAggregator = eventAggregator.ArgumentNotNull(nameof(eventAggregator));
            this.Id = id;
        }

        public Guid Id { get; }

        protected IEventAggregator EventAggregator { get; }
    }
}
