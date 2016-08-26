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

        [Fact]
        public void DeleteEntityDefinition_WithExistingEntityId_RemovesEntity()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);
            target.AddEntityDefinition("initial name", "initial description");

            Guid entityId = target.Schema.Entities.ElementAt(0).Id;

            // act
            target.DeleteEntityDefinition(entityId);

            // assert
            Assert.Equal(0, target.Schema.Entities.Count());
        }

        [Fact]
        public void DeleteEntityDefinition_WithNotExistingEntityId_DoesNotThrow()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);

            // act
            target.DeleteEntityDefinition(Guid.NewGuid());

            // assert
            Assert.Equal(0, target.Schema.Entities.Count());
        }

        [Theory]
        [InlineData("any name", "any description", "any type")]
        [InlineData("any name", "", "any type")]
        public void AddPropertyDefinition_WithValidParameters_AddsModelNode(string name, string description, string propertyType)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);          
            target.AddEntityDefinition("entity", string.Empty);

            // act
            var entity = target.Schema.Entities.First(); 
            target.AddPropertyDefinition(entity.Id, name, description, propertyType);

            // assert
            Assert.Equal(name, entity.Properties.ElementAt(0).Name);
            Assert.Equal(description, entity.Properties.ElementAt(0).Description);
            Assert.Equal(propertyType, entity.Properties.ElementAt(0).PropertyType);
        }

        [Theory]
        [InlineData(null, "any description", "any type")]
        [InlineData("", "any description", "any type")]
        [InlineData("any name", "any description", null)]
        [InlineData("any name", "any description", "")]
        public void AddPropertyDefinition_WithInvalidParameters_ThrowsException(string name, string description, string propertyType)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);          
            target.AddEntityDefinition("entity", string.Empty);

            // act
            var entity = target.Schema.Entities.First(); 
            Assert.ThrowsAny<ArgumentException>(() => target.AddPropertyDefinition(entity.Id, name, description, propertyType));
        }

        [Fact]
        public void AddPropertyDefinition_WithNotExistingEntityId_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);          

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.AddPropertyDefinition(Guid.NewGuid(), "any name", "any description", "any property type"));
        }

        [Theory]
        [InlineData("any name", "any description", "any type")]
        [InlineData("any name", "", "any type")]
        public void ModifPropertyDefinition_WithValidParameters_AddsModelNode(string name, string description, string propertyType)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);
            target.AddEntityDefinition("initial name", "initial description");

            EntityDefinition entity = target.Schema.Entities.ElementAt(0);
            target.AddPropertyDefinition(entity.Id, "initial name", "initial description", "initial property type");

            // act
            PropertyDefinition property = entity.Properties.ElementAt(0);
            target.ModifyPropertyDefinition(property.Id, name, description, propertyType);

            // assert
            Assert.Equal(name, property.Name);
            Assert.Equal(description, property.Description);
            Assert.Equal(propertyType, property.PropertyType);
        }

        [Theory]
        [InlineData(null, "any description", "any type")]
        [InlineData("", "any description", "any type")]
        [InlineData("any name", "any description", null)]
        [InlineData("any name", "any description", "")]
        public void ModifyPropertyDefinition_WithInvalidParameters_ThrowsException(string name, string description, string propertyType)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.ModifyPropertyDefinition(Guid.NewGuid(), name, description, propertyType));
        }

        [Fact]
        public void ModifPropertyDefinition_WithNotExistingPropertyId_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);
            target.AddEntityDefinition("initial name", "initial description");

            EntityDefinition entity = target.Schema.Entities.ElementAt(0);
        
            // act
            Assert.ThrowsAny<ArgumentException>(() => target.ModifyPropertyDefinition(Guid.NewGuid(), "any name", "any description", "any property type"));
        }

        [Fact]
        public void DeletePropertyDefinition_WithExistingPropertyId_RemovesModelNode()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);
            target.AddEntityDefinition("initial name", "initial description");

            EntityDefinition entity = target.Schema.Entities.ElementAt(0);
            target.AddPropertyDefinition(entity.Id, "initial name", "initial description", "initial property type");

            // act
            PropertyDefinition property = entity.Properties.ElementAt(0);
            target.DeletePropertyDefinition(property.Id);

            // assert
            Assert.Equal(0, entity.Properties.Count());
        }

        [Fact]
        public void DeletePropertyDefinition_WithNotExistingPropertyId_DoesNotThrow()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);
            target.AddEntityDefinition("initial name", "initial description");

            // act
            target.DeletePropertyDefinition(Guid.NewGuid());

            // assert
            EntityDefinition entity = target.Schema.Entities.ElementAt(0);
            Assert.Equal(0, entity.Properties.Count());
        }
    }
}
