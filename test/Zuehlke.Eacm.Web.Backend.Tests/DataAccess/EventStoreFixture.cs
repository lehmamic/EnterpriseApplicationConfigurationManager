using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSlite.Events;
using Moq;
using Xunit;
using Zuehlke.Eacm.Web.Backend.DataAccess;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.Tests.FixtureSupport;
using Zuehlke.Eacm.Web.Backend.Utils.Serialization;

namespace Zuehlke.Eacm.Web.Backend.Tests.DataAccess
{
	public class EventStoreFixture : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture context;

        public EventStoreFixture(DatabaseFixture context)
        {
            this.context = context.ArgumentNotNull(nameof(context));
        }

        [Fact]
        public async Task Save_WithEvents_SavesEventsInDbContext()
        {
            // arrange
            var aggregateRootId = Guid.NewGuid();

            var events = new IEvent[]
            {
                new ProjectCreated { Id = aggregateRootId, Name = "AnyName", TimeStamp = DateTimeOffset.Now, Version = 1 },
                new ProjectModified { Id = aggregateRootId, Name = "AnyName", Description = "Any Descrition", TimeStamp = DateTimeOffset.Now, Version = 2 },
            };

            var serializer = new JsonTextSerializer();
            var publisher = new Mock<IEventPublisher>();

            var target = new EventStore(this.context.DbContext, serializer, publisher.Object);

			// act
			await target.Save(events);

            // assert
			Assert.Equal(2, this.context.DbContext.Events.Count(e => e.AggregateId == aggregateRootId));
        }

        [Fact]
        public async Task Get_WithEvents_ReturnsEventsFromDbContext()
        {
            // arrange
            var aggregateRootId = Guid.NewGuid();

            var events = new IEvent[]
            {
                new ProjectCreated { Id = aggregateRootId, Name = "AnyName", TimeStamp = DateTimeOffset.Now, Version = 1 },
                new ProjectModified { Id = aggregateRootId, Name = "AnyName", Description = "Any Descrition", TimeStamp = DateTimeOffset.Now, Version = 2 },
            };

            var serializer = new JsonTextSerializer();
            var publisher = new Mock<IEventPublisher>();

            var target = new EventStore(this.context.DbContext, serializer, publisher.Object);
			await target.Save(events);

            // act
            IEnumerable<IEvent> actual = await target.Get(aggregateRootId, 0);

            // assert
            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public async Task Get_WithFromVersionSmallerThanLatestVersion_ReturnsEventsNewerThanFromVersion()
        {
            // arrange
            var aggregateRootId = Guid.NewGuid();

            var events = new IEvent[]
            {
                new ProjectCreated { Id = aggregateRootId, Name = "AnyName", TimeStamp = DateTimeOffset.Now, Version = 1 },
                new ProjectModified { Id = aggregateRootId, Name = "AnyName", Description = "Any Descrition", TimeStamp = DateTimeOffset.Now, Version = 2 },
            };

            var serializer = new JsonTextSerializer();
            var publisher = new Mock<IEventPublisher>();

            var target = new EventStore(this.context.DbContext, serializer, publisher.Object);
			await target.Save(events);

            // act
            IEnumerable<IEvent> actual = await target.Get(aggregateRootId, 1);

            // assert
            Assert.Equal(1, actual.Count());
        }

        [Fact]
        public async Task Get_WithEventsFromOtherAggregateRoot_ReturnsOnlyEventsFromRequestedAggregateRoot()
        {
			// arrange
			var aggregateRoot1Id = Guid.NewGuid();
            var events1 = new IEvent[]
            {
                new ProjectCreated { Id = aggregateRoot1Id, Name = "AnyName", TimeStamp = DateTimeOffset.Now, Version = 1 },
                new ProjectModified { Id = aggregateRoot1Id, Name = "AnyName", Description = "Any Descrition", TimeStamp = DateTimeOffset.Now, Version = 2 },
            };

			var aggregateRoot2Id = Guid.NewGuid();
			var events2 = new IEvent[]
			{
				new ProjectCreated { Id = aggregateRoot2Id, Name = "AnyName", TimeStamp = DateTimeOffset.Now, Version = 1 },
				new ProjectModified { Id = aggregateRoot2Id, Name = "AnyName", Description = "Any Descrition", TimeStamp = DateTimeOffset.Now, Version = 2 },
			};

            var serializer = new JsonTextSerializer();
            var publisher = new Mock<IEventPublisher>();

            var target = new EventStore(this.context.DbContext, serializer, publisher.Object);
			await target.Save(events1);
			await target.Save(events2);

            // act
            IEnumerable<IEvent> actual = await target.Get(aggregateRoot1Id, 0);

            // assert
            Assert.Equal(2, actual.Count());
        }

        private class TestAggregateRoot
        {
        }
    }
}