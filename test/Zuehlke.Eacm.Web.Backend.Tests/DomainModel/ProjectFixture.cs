using System;
using System.Linq;
using Xunit;
using Zuehlke.Eacm.Web.Backend.DomainModel;

namespace Zuehlke.Eacm.Web.Backend.Tests.DomainModel
{
    public class ProjectFixture
    {
        [Theory]
        [InlineData("any name", "any description")]
        [InlineData("any name", "")]
        public void SetProjectAttribute_WithValidParameters_SetsAttributes(string name, string description)
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

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

            var target = new Project(id, "initial");

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

            var target = new Project(id, "initial");

            // act
            target.AddEntityDefinition(name, description);

            // assert
            Assert.Equal(target.Schema.Entities.Count(), 1);
            Assert.Equal(name, target.Schema.Entities.ElementAt(0).Name);
            Assert.Equal(description, target.Schema.Entities.ElementAt(0).Description);
        }

        [Fact]
        public void AddEntityDefinition_WithValidParameters_AddsEntity()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

            // act
            target.AddEntityDefinition("any name", "any description");

            // assert
            Assert.Equal(target.Configuration.Entities.Count(), 1);
        }

        [Theory]
        [InlineData(null, "any description")]
        [InlineData("", "any description")]
        [InlineData("any name", null)]
        public void AddEntityDefinition_WithInvalidParameters_ThrowsException(string name, string description)
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

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

            var target = new Project(id, "initial");
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

            var target = new Project(id, "initial");

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.ModifyEntityDefinition(Guid.NewGuid(), name, description));
        }

        [Fact]
        public void ModifEntityDefinition_WithNotExistingEntityId_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.ModifyEntityDefinition(Guid.NewGuid(), "name", "description"));
        }

        [Fact]
        public void DeleteEntityDefinition_WithExistingEntityId_RemovesEntityDefinition()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            Guid entityId = target.Schema.Entities.ElementAt(0).Id;

            // act
            target.DeleteEntityDefinition(entityId);

            // assert
            Assert.Equal(0, target.Schema.Entities.Count());
        }

        [Fact]
        public void DeleteEntityDefinition_WithExistingEntityId_RemovesEntity()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            Guid entityId = target.Schema.Entities.ElementAt(0).Id;

            // act
            target.DeleteEntityDefinition(entityId);

            // assert
            Assert.Equal(0, target.Configuration.Entities.Count());
        }

        [Fact]
        public void DeleteEntityDefinition_WithNotExistingEntityId_DoesNotThrow()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

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

            var target = new Project(id, "initial");
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

            var target = new Project(id, "initial");
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

            var target = new Project(id, "initial");

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

            var target = new Project(id, "initial");
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

            var target = new Project(id, "initial");

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.ModifyPropertyDefinition(Guid.NewGuid(), name, description, propertyType));
        }

        [Fact]
        public void ModifPropertyDefinition_WithNotExistingPropertyId_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.ModifyPropertyDefinition(Guid.NewGuid(), "any name", "any description", "any property type"));
        }

        [Fact]
        public void DeletePropertyDefinition_WithExistingPropertyId_RemovesModelNode()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "Initial");
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

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            // act
            target.DeletePropertyDefinition(Guid.NewGuid());

            // assert
            EntityDefinition entity = target.Schema.Entities.ElementAt(0);
            Assert.Equal(0, entity.Properties.Count());
        }

        [Fact]
        public void AddEntry_WithValidParameters_AddsEntryToTheEntity()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            // act
            target.AddEntry(entity.Id, new object[] { "Test", 10 });

            // assert
            Assert.Equal(1, target.Configuration[entity.Id].Entries.Count());
            Assert.Equal(2, target.Configuration[entity.Id].Entries.ElementAt(0).Values.Count());
            Assert.Equal("Key", target.Configuration[entity.Id].Entries.ElementAt(0).Values.ElementAt(0).Property.Name);
            Assert.Equal("Test", target.Configuration[entity.Id].Entries.ElementAt(0).Values.ElementAt(0).Value);
            Assert.Equal("Value", target.Configuration[entity.Id].Entries.ElementAt(0).Values.ElementAt(1).Property.Name);
            Assert.Equal(10, target.Configuration[entity.Id].Entries.ElementAt(0).Values.ElementAt(1).Value);
        }

        [Fact]
        public void AddEntry_WithInvalidEntityId_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.AddEntry(Guid.NewGuid(), new object[] { "Test", 10 }));
        } 

        [Fact]
        public void AddEntry_WithInvalidAmountOfValues_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.AddEntry(entity.Id, new object[] { "Test" }));
        }

        [Fact]
        public void AddEntry_WithValuesAreNull_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.AddEntry(entity.Id, null));
        }

        [Fact]
        public void ModifyEntry_WithValidParameters_ModifiesEntryValues()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");
            target.AddEntry(entity.Id, new object[] { "Test", 10 });

            // act
            var entry = target.Configuration.Entities.First().Entries.First();
            target.ModifyEntry(entry.Id, new object[] { "AnotherTest", 23 });

            // assert
            Assert.Equal(1, target.Configuration[entity.Id].Entries.Count());
            Assert.Equal(2, target.Configuration[entity.Id].Entries.ElementAt(0).Values.Count());
            Assert.Equal("Key", target.Configuration[entity.Id].Entries.ElementAt(0).Values.ElementAt(0).Property.Name);
            Assert.Equal("AnotherTest", target.Configuration[entity.Id].Entries.ElementAt(0).Values.ElementAt(0).Value);
            Assert.Equal("Value", target.Configuration[entity.Id].Entries.ElementAt(0).Values.ElementAt(1).Property.Name);
            Assert.Equal(23, target.Configuration[entity.Id].Entries.ElementAt(0).Values.ElementAt(1).Value);
        }

        [Fact]
        public void ModifyEntry_WithMultipleEntries_ModifiesCorrectEntry()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");
            target.AddEntry(entity.Id, new object[] { "Test1", 10 });
            target.AddEntry(entity.Id, new object[] { "Test2", 20 });

            // act
            var entry1 = target.Configuration.Entities.First().Entries.ElementAt(0);
            var entry2 = target.Configuration.Entities.First().Entries.ElementAt(1);
            target.ModifyEntry(entry2.Id, new object[] { "AnotherTest", 23 });

            // assert
            Assert.Equal(2, target.Configuration[entity.Id].Entries.Count());

            Assert.Equal("Test1", entry1.Values.ElementAt(0).Value);
            Assert.Equal(10, entry1.Values.ElementAt(1).Value);

            Assert.Equal("AnotherTest", entry2.Values.ElementAt(0).Value);
            Assert.Equal(23, entry2.Values.ElementAt(1).Value);
        }

        [Fact]
        public void ModifyEntry_WithValuesAreNull_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            // act
            Assert.ThrowsAny<ArgumentNullException>(() => target.ModifyEntry(Guid.NewGuid(), null));
        }

        [Fact]
        public void ModifyEntry_WithValuesCountDoesNotMatchPropertyAmount_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.ModifyEntry(Guid.NewGuid(), new object[] { 23 }));
        }

        [Fact]
        public void ModifyEntry_WithNotExistingEntryId_ThrowsException()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");
            target.AddEntityDefinition("initial name", "initial description");

            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            // act
            Assert.ThrowsAny<ArgumentException>(() => target.ModifyEntry(Guid.NewGuid(), new object[] { "AnotherTest", 23 }));
        }

        [Fact]
        public void DeleteEntry_WithExistingEntry_RemovesTheEntryFromTheEntity()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

            target.AddEntityDefinition("initial name", "initial description");
            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            target.AddEntry(entity.Id, new object[] { "Test", 10 });
            var entry = target.Configuration[entity.Id].Entries.First();

            // act
            target.DeleteEntry(entry.Id);

            // assert
            Assert.Equal(0, target.Configuration[entity.Id].Entries.Count());
        }

        [Fact]
        public void DeleteEntry_WithMultipleEntries_RemovesTheCorrectEntry()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

            target.AddEntityDefinition("initial name", "initial description");
            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            target.AddEntry(entity.Id, new object[] { "Test1", 10 });
            target.AddEntry(entity.Id, new object[] { "Test2", 20 });
            var entry1 = target.Configuration[entity.Id].Entries.ElementAt(0);
            var entry2 = target.Configuration[entity.Id].Entries.ElementAt(1);

            // act
            target.DeleteEntry(entry2.Id);

            // assert
            Assert.Equal(1, target.Configuration[entity.Id].Entries.Count());
            Assert.Equal(entry1.Id, target.Configuration[entity.Id].Entries.First().Id);
        }

        [Fact]
        public void DeleteEntry_WithNotExistingEntryId_DoesNotThrow()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

            target.AddEntityDefinition("initial name", "initial description");
            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            // act
            target.DeleteEntry(Guid.NewGuid());

            // assert
            Assert.Equal(0, target.Configuration[entity.Id].Entries.Count());
        }

        [Fact]
        public void AddPropertyDefinition_WithExistingEntries_AddsConfigurationValueForProperty()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

            target.AddEntityDefinition("initial name", "initial description");
            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddEntry(entity.Id, new object[] { "Test" });
            var entry = target.Configuration[entity.Id].Entries.First();

            // act
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            // assert
            Assert.Equal(2, entry.Values.Count());
            Assert.Equal("Value", target.Configuration[entity.Id].Entries.ElementAt(0).Values.ElementAt(1).Property.Name);
            Assert.Equal(null, target.Configuration[entity.Id].Entries.ElementAt(0).Values.ElementAt(1).Value);
        }

        [Fact]
        public void ModifyPropertyDefinition_WithExistingEntriesAndNameChanged_DoesChangeConfigurationValueForProperty()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

            target.AddEntityDefinition("initial name", "initial description");
            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            target.AddEntry(entity.Id, new object[] { "Test", 10 });
            var entry = target.Configuration[entity.Id].Entries.First();

            // act
            PropertyDefinition property = entity.Properties.Single(p => p.Name == "Value");
            target.ModifyPropertyDefinition(property.Id, "Value2", "", "Zuehlke.Eacm.Integer");

            // assert
            Assert.Equal(2, entry.Values.Count());
            Assert.Equal("Value2", entry.Values.ElementAt(1).Property.Name);
            Assert.Equal(10, entry.Values.ElementAt(1).Value);
        }

        [Fact]
        public void ModifyPropertyDefinition_WithExistingEntriesAndTypeChanged_ResetsConfigurationValueForProperty()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

            target.AddEntityDefinition("initial name", "initial description");
            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            target.AddEntry(entity.Id, new object[] { "Test", 10 });
            var entry = target.Configuration[entity.Id].Entries.First();

            // act
            PropertyDefinition property = entity.Properties.Single(p => p.Name == "Value");
            target.ModifyPropertyDefinition(property.Id, "Value", "", "Zuehlke.Eacm.String");

            // assert
            Assert.Equal(2, entry.Values.Count());
            Assert.Equal("Zuehlke.Eacm.String", entry.Values.ElementAt(1).Property.PropertyType);
            Assert.Equal(null, entry.Values.ElementAt(1).Value);
        }

        [Fact]
        public void DeletePropertyDefinition_WithExistingEntries_RemovesConfigurationValueForProperty()
        {
            // arrange
            var id = Guid.NewGuid();

            var target = new Project(id, "initial");

            target.AddEntityDefinition("initial name", "initial description");
            var entity = target.Schema.Entities.First();

            target.AddPropertyDefinition(entity.Id, "Key", string.Empty, "Zuehlke.Eacm.String");
            target.AddPropertyDefinition(entity.Id, "Value", string.Empty, "Zuehlke.Eacm.Integer");

            target.AddEntry(entity.Id, new object[] { "Test", 10 });
            var entry = target.Configuration[entity.Id].Entries.First();

            // act
            PropertyDefinition property = entity.Properties.Single(p => p.Name == "Value");
            target.DeletePropertyDefinition(property.Id);

            // assert
            Assert.Equal(1, entry.Values.Count());
            Assert.False(entry.Definition.Properties.Any(p => p.Name == "Value"));
        }
    }
}
