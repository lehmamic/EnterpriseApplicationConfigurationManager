﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.CQRS
{
    public abstract class EventSourced : IEventSourced
    {
        private static readonly DateTime InitialCreatedValue = DateTime.MinValue;

        private readonly Dictionary<Type, Action<IEvent>> handlers = new Dictionary<Type, Action<IEvent>>();
        private readonly Collection<IEvent> pendingEvents = new Collection<IEvent>();

        protected EventSourced(Guid id)
        {
            this.Id = id;
        }

        #region Implementation of IAggregateRoot
        public Guid Id { get; }
        #endregion

        #region Implementation of IEventSourced
        public DateTime Created { get; private set; } = InitialCreatedValue;

        public DateTime? Modified { get; private set; }

        public IEventAggregator EventAggregator { get; } = new EventAggregator();

        public IEnumerable<IEvent> PendingEvents => this.pendingEvents.ToImmutableArray();
        #endregion

        protected void LoadFrom(IEnumerable<IEvent> pastEvents)
        {
            pastEvents.ArgumentNotNull(nameof(pastEvents));

            foreach (var e in pastEvents.ToImmutableArray().OrderBy(e => e.Timestamp))
            {
                this.HandleEvent(e);
            }
        }

        protected void Update(IEvent e)
        {
            e.ArgumentNotNull(nameof(e));

            e.SourceId = this.Id;
            e.Timestamp = DateTime.UtcNow;

            this.HandleEvent(e);
            this.pendingEvents.Add(e);
        }

        private void HandleEvent(IEvent e)
        {
            this.EventAggregator.PublishEvent(e);

            if (this.IsNew())
            {
                this.Created = e.Timestamp;
            }
            else
            {
                this.Modified = e.Timestamp;
            }
        }

        private bool IsNew()
        {
            return this.Created == InitialCreatedValue;
        }
    }
}