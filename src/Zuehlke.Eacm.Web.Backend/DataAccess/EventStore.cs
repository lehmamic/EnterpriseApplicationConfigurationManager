using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CQRSlite.Events;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.Utils.Serialization;

namespace Zuehlke.Eacm.Web.Backend.DataAccess
{
    public class EventStore : IEventStore
    {
        private readonly ITextSerializer serializer;
        private readonly EacmDbContext dbContext;
        private readonly IEventPublisher publisher;

        public EventStore(EacmDbContext dbContext, ITextSerializer serializer, IEventPublisher publisher)
        {
            this.dbContext = dbContext.ArgumentNotNull(nameof(dbContext));
            this.serializer = serializer.ArgumentNotNull(nameof(serializer));
            this.publisher = publisher.ArgumentNotNull(nameof(publisher));
        }

        public void Save<T>(IEnumerable<IEvent> events)
        {
            events.ArgumentNotNull(nameof(events));

            foreach (var e in events)
            {
                var @event = new Event
                {
                    Id = Guid.NewGuid(),
                    AggregateId = e.Id,
                    AggregateType = typeof(T).Name,
                    Timestamp = e.TimeStamp,
                    Version = e.Version,
                    Payload = this.Serialize(e)
                };

                this.dbContext.Events.Add(@event);
                this.dbContext.SaveChanges();

                this.publisher.Publish(e);
            }
        }

        public IEnumerable<IEvent> Get<T>(Guid aggregateId, int fromVersion)
        {
            return this.dbContext.Events
                .Where(e => e.AggregateId == aggregateId && e.AggregateType == typeof(T).Name)
                .Where(e => e.Version > fromVersion)
                .Select(e => this.Deserialize(e.Payload))
                .OrderByDescending(e => e.Version);
        }

        private string Serialize(IEvent @event)
        {
            using (var writer = new StringWriter())
            {
                this.serializer.Serialize(writer, @event);
                return writer.ToString();
            }
        }

        private IEvent Deserialize(string payload)
        {
            using (var reader = new StringReader(payload))
            {
                return (IEvent)this.serializer.Deserialize(reader);
            }
        }
    }
}