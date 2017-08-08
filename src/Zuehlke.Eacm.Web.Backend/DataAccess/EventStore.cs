using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
		{       
			events.ArgumentNotNull(nameof(events));

            foreach (var e in events)
            {
                var @event = new Event
                {
                    Id = Guid.NewGuid(),
                    AggregateId = e.Id,
                    Timestamp = e.TimeStamp,
                    Version = e.Version,
                    User = "DummyUser",
                    Payload = this.Serialize(e)
                };

				await this.dbContext.Events.AddAsync(@event);
                await this.dbContext.SaveChangesAsync();

                await this.publisher.Publish(e);
            }
		}

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await this.dbContext.Events
                .Where(e => e.AggregateId == aggregateId)
                .Where(e => e.Version > fromVersion)
                .Select(e => this.Deserialize(e.Payload))
				.OrderByDescending(e => e.Version)
				.ToAsyncEnumerable()
				.ToList();
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