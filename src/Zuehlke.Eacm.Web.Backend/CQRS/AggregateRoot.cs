using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Zuehlke.Eacm.Web.Backend.Diagnostics;

namespace Zuehlke.Eacm.Web.Backend.CQRS
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        #region Implementation of IAggregateRoot
        public Guid Id { get; set; }

        public DateTime Created { get; protected set; }

        public DateTime? Modified { get; protected set; }

        public void HandleEvents(IEnumerable<IEvent> events)
        {
            events.ArgumentNotNull(nameof(events))
                  .ItemsNotNull(nameof(events))
                  .ExpectedCondition(a => a.All(e => e.Id == this.Id), $"Not all events have the same source id like the aggregate {this.Id}.", nameof(events));
            
            foreach (var e in events)
            {
                this.GetType().GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(e.GetType())
                    .Invoke(this, new object[] { e });
            }
        }
        #endregion

        protected void Update<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            @event.ArgumentNotNull(nameof(@event));

            var handler = this as IEventHandler<TEvent>;
            if (handler == null)
            {
                throw new ArgumentException($"Aggregate {this.GetType().Name} does not know how to apply event {@event.GetType().Name}", nameof(@event));
            }

            handler.Handle(@event);
        }
    }
}