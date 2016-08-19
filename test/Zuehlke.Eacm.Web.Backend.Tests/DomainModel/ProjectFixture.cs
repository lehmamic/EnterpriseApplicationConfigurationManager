using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Zuehlke.Eacm.Web.Backend.CQRS;
using Zuehlke.Eacm.Web.Backend.DomainModel;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;

namespace Zuehlke.Eacm.Web.Backend.Tests.DomainModel
{
    public class ProjectFixture
    {
        [Fact]
        public void Constructor_WithEmptyHistory_CreatedDateNotInitialized()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            // act
            var target = new Project(id, history);

            // assert
            Assert.Equal(DateTime.MinValue, target.Created);
        }

        [Fact]
        public void Constructor_WithEmptyHistory_ModifiedDateNotInitialized()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            // act
            var target = new Project(id, history);

            // assert
            Assert.Null(target.Modified);
        }

        [Fact]
        public void Constructor_WithEmptyHistory_NameNotInitialized()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            // act
            var target = new Project(id, history);

            // assert
            Assert.Null(target.Name);
        }

        [Fact]
        public void Constructor_WithEmptyHistory_DescritionNotInitialized()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            // act
            var target = new Project(id, history);

            // assert
            Assert.Null(target.Description);
        }

        [Fact]
        public void Constructor_WithHistory_AppliesTheEvents()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>
            {
                new ProjectAttributesChanged { SourceId = id, CorrelationId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "AnyName", Description = "AnyDescription" },
            };

            // act
            var target = new Project(id, history);

            // assert
            Assert.Equal("AnyName", target.Name);
            Assert.Equal("AnyDescription", target.Description);
        }

        [Fact]
        public void Constructor_WithHistory_AppliesTheEventsInCorrectOrder()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>
            {
                new ProjectAttributesChanged { SourceId = id, CorrelationId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "FirstName", Description = "FirstDescription" },
                new ProjectAttributesChanged { SourceId = id, CorrelationId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "SecondName", Description = "SecondDescription" },
            };

            // act
            var target = new Project(id, history);

            // assert
            Assert.Equal("SecondName", target.Name);
            Assert.Equal("SecondDescription", target.Description);
        }

        [Fact]
        public void Constructor_WithHistoryEventIdsNotMatchingWithModelId_ThrowsException()
        {
            // arrange
            var modelId = Guid.NewGuid();
            var eventsId = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>
            {
                new ProjectAttributesChanged { SourceId = eventsId, CorrelationId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "AnyName", Description = "AnyDescription" },
                new ProjectAttributesChanged { SourceId = eventsId, CorrelationId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "AnyName", Description = "AnyDescription" },
                new ProjectAttributesChanged { SourceId = eventsId, CorrelationId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "AnyName", Description = "AnyDescription" },
            };

            // act
            Assert.ThrowsAny<ArgumentException>(() => new Project(modelId, history));
        }

        [Fact]
        public void Constructor_WithHistoryEventIdsNotEqualWithModelId_ThrowsException()
        {
            // arrange
            var modelId = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>
            {
                new ProjectAttributesChanged { SourceId = modelId, CorrelationId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "AnyName", Description = "AnyDescription" },
                new ProjectAttributesChanged { SourceId = Guid.NewGuid(), CorrelationId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "AnyName", Description = "AnyDescription" },
                new ProjectAttributesChanged { SourceId = modelId, CorrelationId = Guid.NewGuid(), Id = Guid.NewGuid(), Name = "AnyName", Description = "AnyDescription" },
            };

            // act
            Assert.ThrowsAny<ArgumentException>(() => new Project(modelId, history));
        }

        [Theory]
        [InlineData("any name", "any description")]
        [InlineData("any name", "")]
        public void SetProjectAttribute_WithValidParameters_SetsAttributes(string name, string description)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);

            // act
            target.SetProjectAttributes(name, description);

            // assert
            Assert.Equal(name, target.Name);
            Assert.Equal(description, target.Description);
        }

        [Theory]
        [InlineData(null, "any description")]
        [InlineData("", "any description")]
        [InlineData("any name", null)]
        public void SetProjectAttribute_WithInvalidParameters_ThrowsException(string name, string description)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.SetProjectAttributes(name, description));
        }

        [Theory]
        [InlineData("any name", "any description")]
        [InlineData("any name", "")]
        public void AddEntityDefinition_WithValidParameters_AddsModelNode(string name, string description)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);

            // act
            target.AddEntityDefinition(name, description);

            // assert
            Assert.Equal(target.Schema.Entities.Count(), 1);
            Assert.Equal(name, target.Schema.Entities.ElementAt(0).Name);
            Assert.Equal(description, target.Schema.Entities.ElementAt(0).Description);
        }

        [Theory]
        [InlineData(null, "any description")]
        [InlineData("", "any description")]
        [InlineData("any name", null)]
        public void AddEntityDefinition_WithInvalidParameters_ThrowsException(string name, string description)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.AddEntityDefinition(name, description));
        }

        [Theory]
        [InlineData("any name", "any description")]
        [InlineData("any name", "")]
        public void ModifEntityDefinition_WithValidParameters_AddsModelNode(string name, string description)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);
            target.AddEntityDefinition("initial name", "initial description");

            Guid entityId = target.Schema.Entities.ElementAt(0).Id;

            // act
            target.ModifyEntityDefinition(entityId, name, description);

            // assert
            Assert.Equal(name, target.Schema.Entities.ElementAt(0).Name);
            Assert.Equal(description, target.Schema.Entities.ElementAt(0).Description);
        }

        [Theory]
        [InlineData(null, "any description")]
        [InlineData("", "any description")]
        [InlineData("any name", null)]
        public void ModifyEntityDefinition_WithInvalidParameters_ThrowsException(string name, string description)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.ModifyEntityDefinition(Guid.NewGuid(), name, description));
        }

        [Fact]
        public void ModifEntityDefinition_WithNotExistingEntityId_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.ModifyEntityDefinition(Guid.NewGuid(), "name", "description"));
        }
    }
}
