using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Events;
using Moq;
using Xunit;
using Zuehlke.Eacm.Web.Backend.DataAccess;
using Zuehlke.Eacm.Web.Backend.DomainModel;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.Tests.FixtureSupport;
using Zuehlke.Eacm.Web.Backend.Utils.Serialization;

namespace Zuehlke.Eacm.Web.Backend.Tests.DataAccess
{
    public class EventStoreFixture : DatabaseFixture
    {
        [Fact]
        public void Save_WithEvents_SavesEventsInDbContext()
        {
            // arrange
            var aggregateRootId = Guid.NewGuid();

            var events = new IEvent[]
            {
                new ProjectCreated { Id = aggregateRootId, Name = "AnyName", TimeStamp = DateTimeOffset.Now, Version = 1 },
                new ProjectAttributesModified { Id = aggregateRootId, Name = "AnyName", Description = "Any Descrition", TimeStamp = DateTimeOffset.Now, Version = 2 },
            };

            var serializer = new JsonTextSerializer();
            var publisher = new Mock<IEventPublisher>();

            var target = new EventStore(this.DbContext, serializer, publisher.Object);

            // act
            target.Save<Project>(events);

            // assert
            Assert.Equal(2, this.DbContext.Events.Count());
        }

        [Fact]
        public void Get_WithEvents_ReturnsEventsFromDbContext()
        {
            // arrange
            var aggregateRootId = Guid.NewGuid();

            var events = new IEvent[]
            {
                new ProjectCreated { Id = aggregateRootId, Name = "AnyName", TimeStamp = DateTimeOffset.Now, Version = 1 },
                new ProjectAttributesModified { Id = aggregateRootId, Name = "AnyName", Description = "Any Descrition", TimeStamp = DateTimeOffset.Now, Version = 2 },
            };

            var serializer = new JsonTextSerializer();
            var publisher = new Mock<IEventPublisher>();

            var target = new EventStore(this.DbContext, serializer, publisher.Object);
            target.Save<Project>(events);

            // act
            IEnumerable<IEvent> actual = target.Get<Project>(aggregateRootId, 0);

            // assert
            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public void Get_WithFromVersionSmallerThanLatestVersion_ReturnsEventsNewerThanFromVersion()
        {
            // arrange
            var aggregateRootId = Guid.NewGuid();

            var events = new IEvent[]
            {
                new ProjectCreated { Id = aggregateRootId, Name = "AnyName", TimeStamp = DateTimeOffset.Now, Version = 1 },
                new ProjectAttributesModified { Id = aggregateRootId, Name = "AnyName", Description = "Any Descrition", TimeStamp = DateTimeOffset.Now, Version = 2 },
            };

            var serializer = new JsonTextSerializer();
            var publisher = new Mock<IEventPublisher>();

            var target = new EventStore(this.DbContext, serializer, publisher.Object);
            target.Save<Project>(events);

            // act
            IEnumerable<IEvent> actual = target.Get<Project>(aggregateRootId, 1);

            // assert
            Assert.Equal(1, actual.Count());
        }
    }
}