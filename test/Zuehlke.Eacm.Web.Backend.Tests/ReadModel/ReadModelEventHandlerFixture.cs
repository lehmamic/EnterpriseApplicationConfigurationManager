using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;
using Zuehlke.Eacm.Web.Backend.DataAccess;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.ReadModel;
using Zuehlke.Eacm.Web.Backend.Tests.FixtureSupport;

namespace Zuehlke.Eacm.Web.Backend.Tests.ReadModel
{
    public class ReadModelEventHandlerFixture : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture context;

        public ReadModelEventHandlerFixture(DatabaseFixture context)
        {
            this.context = context.ArgumentNotNull(nameof(context));
        }

        [Fact]
        public void Handle_ProjectCreatedEvent_CreatesProject()
        {
            // arrange
            var message = new ProjectCreated
            {
                Id = Guid.NewGuid(),
                Version = 1,
                TimeStamp = DateTimeOffset.Now,
                Name = "New Project"
            };

            var target = new ReadModelEventHandler(this.context.DbContext, this.context.Mapper);

            // act
            target.Handle(message);

            // assert
            var project = this.context.DbContext.Projects.FirstOrDefault(p => p.Id == message.Id);
            Assert.NotNull(project);
            Assert.Equal(message.Version, project.Version);
            Assert.Equal(message.TimeStamp, project.TimeStamp);
            Assert.Equal(message.Name, project.Name);
            Assert.Null(project.Description);
        }

        [Fact]
        public void Handle_ProjectModifiedEvent_CreatesProject()
        {
            // arrange
            var initialProject = this.CreateProject();

            var message = new ProjectModified
            {
                Id = initialProject.Id,
                Version = 2,
                TimeStamp = DateTimeOffset.Now,
                Name = "New Project name",
                Description = "New Project description"
            };

            var target = new ReadModelEventHandler(this.context.DbContext, this.context.Mapper);

            // act
            target.Handle(message);

            // assert
            var project = this.context.DbContext.Projects.FirstOrDefault(p => p.Id == message.Id);
            Assert.NotNull(project);
            Assert.Equal(message.Version, project.Version);
            Assert.Equal(message.TimeStamp, project.TimeStamp);
            Assert.Equal(message.Name, project.Name);
            Assert.Equal(message.Description, project.Description);
        }

        [Fact]
        public void Handle_EntityDefinitionAddedEvent_CreatesEntity()
        {
            // arrange
            var initialProject = this.CreateProject();

            var message = new EntityDefinitionAdded
            {
                Id = initialProject.Id,
                EntityId = Guid.NewGuid(),
                Version = 2,
                TimeStamp = DateTimeOffset.Now,
                Name = "New Entity Name",
                Description = "new Entity Description"
            };

            var target = new ReadModelEventHandler(this.context.DbContext, this.context.Mapper);

            // act
            target.Handle(message);

            // assert
            var project = this.context.DbContext.Projects.First(p => p.Id == message.Id);
            Assert.Equal(message.Id, project.Id);
            Assert.Equal(message.Version, project.Version);
            Assert.Equal(message.TimeStamp, project.TimeStamp);

            var entity = this.context.DbContext.Entities.FirstOrDefault(p => p.Id == message.EntityId);
            Assert.NotNull(entity);
            Assert.Equal(message.EntityId, entity.Id);
            Assert.Equal(message.Id, entity.ProjectId);
            Assert.Equal(message.Name, entity.Name);
            Assert.Equal(message.Description, entity.Description);
        }

        [Fact]
        public void Handle_EntityDefinitionModifiedEvent_CreatesEntity()
        {
            // arrange
            var initialProject = this.CreateProject();
            var initialEntity = initialProject.Entities.First();

            var message = new EntityDefinitionModified
            {
                Id = initialProject.Id,
                EntityId = initialEntity.Id,
                Version = 2,
                TimeStamp = DateTimeOffset.Now,
                Name = "New Entity Name",
                Description = "new Entity Description"
            };

            var target = new ReadModelEventHandler(this.context.DbContext, this.context.Mapper);

            // act
            target.Handle(message);

            // assert
            var project = this.context.DbContext.Projects.First(p => p.Id == message.Id);
            Assert.Equal(message.Id, project.Id);
            Assert.Equal(message.Version, project.Version);
            Assert.Equal(message.TimeStamp, project.TimeStamp);

            var entity = this.context.DbContext.Entities.FirstOrDefault(p => p.Id == message.EntityId);
            Assert.NotNull(entity);
            Assert.Equal(message.EntityId, entity.Id);
            Assert.Equal(message.Id, entity.ProjectId);
            Assert.Equal(message.Name, entity.Name);
            Assert.Equal(message.Description, entity.Description);
        }

        [Fact]
        public void Handle_EntityDefinitionDeletedEvent_RemovesEntity()
        {
            // arrange
            var initialProject = this.CreateProject();
            var initialEntity = initialProject.Entities.First();

            var message = new EntityDefinitionDeleted
            {
                Id = initialProject.Id,
                EntityId = initialEntity.Id,
                Version = 2,
                TimeStamp = DateTimeOffset.Now
            };

            var target = new ReadModelEventHandler(this.context.DbContext, this.context.Mapper);

            // act
            target.Handle(message);

            // assert
            var project = this.context.DbContext.Projects.First(p => p.Id == message.Id);
            Assert.Equal(message.Id, project.Id);
            Assert.Equal(message.Version, project.Version);
            Assert.Equal(message.TimeStamp, project.TimeStamp);

            var entity = this.context.DbContext.Entities.FirstOrDefault(p => p.Id == message.EntityId);
            Assert.Null(entity);
        }

        [Fact]
        public void Handle_PropertyDefinitionAddedEvent_CreatesProperty()
        {
            // arrange
            var initialProject = this.CreateProject();
            var initialEntity = initialProject.Entities.First();

            var message = new PropertyDefinitionAdded
            {
                Id = initialProject.Id,
                ParentEntityId = initialEntity.Id,
                PropertyId = Guid.NewGuid(),
                Name = "New property name",
                Description = "New property description",
                Version = 2,
                TimeStamp = DateTimeOffset.Now
            };

            var target = new ReadModelEventHandler(this.context.DbContext, this.context.Mapper);

            // act
            target.Handle(message);

            // assert
            var project = this.context.DbContext.Projects.First(p => p.Id == message.Id);
            Assert.Equal(message.Id, project.Id);
            Assert.Equal(message.Version, project.Version);
            Assert.Equal(message.TimeStamp, project.TimeStamp);

            var property = this.context.DbContext.Properties.FirstOrDefault(p => p.Id == message.PropertyId);
            Assert.NotNull(property);
            Assert.Equal(message.ParentEntityId, property.EntityId);
            Assert.Equal(message.Name, property.Name);
            Assert.Equal(message.Description, property.Description);
        }

        [Fact]
        public void Handle_PropertyDefinitionModifiedEvent_CreatesProperty()
        {
            // arrange
            var initialProject = this.CreateProject();
            var initialProperty = initialProject.Entities.First().Properties.First();

            var message = new PropertyDefinitionModified
            {
                Id = initialProject.Id,
                PropertyId = initialProperty.Id,
                Name = "New property name",
                Description = "New property description",
                PropertyType = "Zuehlke.Eacm.String",
                Version = 2,
                TimeStamp = DateTimeOffset.Now
            };

            var target = new ReadModelEventHandler(this.context.DbContext, this.context.Mapper);

            // act
            target.Handle(message);

            // assert
            var project = this.context.DbContext.Projects.First(p => p.Id == message.Id);
            Assert.Equal(message.Id, project.Id);
            Assert.Equal(message.Version, project.Version);
            Assert.Equal(message.TimeStamp, project.TimeStamp);

            var property = this.context.DbContext.Properties.FirstOrDefault(p => p.Id == message.PropertyId);
            Assert.NotNull(property);
            Assert.Equal(message.Name, property.Name);
            Assert.Equal(message.Description, property.Description);
            Assert.Equal(message.PropertyType, property.PropertyType);
        }

        [Fact]
        public void Handle_PropertyDefinitionDeletedEvent_CreatesProperty()
        {
            // arrange
            var initialProject = this.CreateProject();
            var initialProperty = initialProject.Entities.First().Properties.First();

            var message = new PropertyDefinitionDeleted
            {
                Id = initialProject.Id,
                PropertyId = initialProperty.Id,
                Version = 2,
                TimeStamp = DateTimeOffset.Now
            };

            var target = new ReadModelEventHandler(this.context.DbContext, this.context.Mapper);

            // act
            target.Handle(message);

            // assert
            var project = this.context.DbContext.Projects.First(p => p.Id == message.Id);
            Assert.Equal(message.Id, project.Id);
            Assert.Equal(message.Version, project.Version);
            Assert.Equal(message.TimeStamp, project.TimeStamp);

            var property = this.context.DbContext.Properties.FirstOrDefault(p => p.Id == message.PropertyId);
            Assert.Null(property);
        }

        [Fact]
        public void Handle_ConfigurationEntryAddedEvent_CreatesEntry()
        {
            // arrange
            var initialProject = this.CreateProject();
            var initialEntity = initialProject.Entities.First();
            var initialProperty = initialEntity.Properties.First();

            var message = new ConfigurationEntryAdded
            {
                Id = initialProject.Id,
                EntryId = Guid.NewGuid(),
                EntityId = initialEntity.Id,
                Values = new Dictionary<Guid, object> {{ initialProperty.Id, "TestValue" }},
                Version = 2,
                TimeStamp = DateTimeOffset.Now
            };

            var target = new ReadModelEventHandler(this.context.DbContext, this.context.Mapper);

            // act
            target.Handle(message);

            // assert
            var project = this.context.DbContext.Projects.First(p => p.Id == message.Id);
            Assert.Equal(message.Id, project.Id);
            Assert.Equal(message.Version, project.Version);
            Assert.Equal(message.TimeStamp, project.TimeStamp);

            var entry = this.context.DbContext.Entries.FirstOrDefault(p => p.Id == message.EntryId);
            Assert.NotNull(entry);

            var value = this.context.DbContext.Values.FirstOrDefault(p => p.EntryId == message.EntryId);
            Assert.NotNull(value);
            Assert.Equal(initialProperty.Id, value.PropertyId);
            Assert.Equal("TestValue", value.Value);
        }

        [Fact]
        public void Handle_ConfigurationEntryModifiedEvent_CreatesEntry()
        {
            // arrange
            var initialProject = this.CreateProject();
            var initialEntity = initialProject.Entities.First();
            var initialEntry = initialEntity.Entries.First();

            var message = new ConfigurationEntryModified
            {
                Id = initialProject.Id,
                EntryId = initialEntry.Id,
                Values = new Dictionary<Guid, object>
                {
                    { initialEntity.Properties.ElementAt(0).Id, "TestValue1" },
                    { initialEntity.Properties.ElementAt(1).Id, "TestValue2" }
                },
                Version = 2,
                TimeStamp = DateTimeOffset.Now
            };

            var target = new ReadModelEventHandler(this.context.DbContext, this.context.Mapper);

            // act
            target.Handle(message);

            // assert
            var project = this.context.DbContext.Projects.First(p => p.Id == message.Id);
            Assert.Equal(message.Id, project.Id);
            Assert.Equal(message.Version, project.Version);
            Assert.Equal(message.TimeStamp, project.TimeStamp);

            var values = this.context.DbContext.Values.Where(p => p.EntryId == message.EntryId);

            Assert.Equal(2, values.Count());
            Assert.Contains(values, v => v.Value == "TestValue1");
            Assert.Contains(values, v => v.Value == "TestValue2");
        }

        private ConfigurationProject CreateProject()
        {
            var project = new ConfigurationProject
            {
                Id = Guid.NewGuid(),
                Version = 1,
                TimeStamp = DateTimeOffset.Now.AddDays(-1),
                Name = "Initial project name"
            };

            var entity = new ConfigurationEntity
            {
                Id = Guid.NewGuid(),
                Name = "Initial entity name 1",
                Description = "Initial entity description",
                ProjectId = project.Id
            };

            project.Entities = new Collection<ConfigurationEntity>{entity };

            entity.Properties = new Collection<ConfigurationProperty>
            {
                new ConfigurationProperty
                {
                    Id = Guid.NewGuid(),
                    Name = "TestProperty",
                    PropertyType = "Zuehlke.Eacm.Integer",
                    EntityId = entity.Id
                },
                new ConfigurationProperty
                {
                    Id = Guid.NewGuid(),
                    Name = "TestPropertyToDelete",
                    PropertyType = "Zuehlke.Eacm.Integer",
                    EntityId = entity.Id
                }
            };

            entity.Entries = new Collection<ConfigurationEntry>
            {
                new ConfigurationEntry
                {
                    Id = Guid.NewGuid(),
                    EntityId = entity.Id
                }
            };

            var entry = entity.Entries.First();
            entry.Values = new Collection<ConfigurationValue>
            {
                new ConfigurationValue
                {
                    Id = Guid.NewGuid(),
                    EntryId = entry.Id,
                    PropertyId = entity.Properties.ElementAt(0).Id,
                    Value = "First Value"
                },
                new ConfigurationValue
                {
                    Id = Guid.NewGuid(),
                    EntryId = entry.Id,
                    PropertyId = entity.Properties.ElementAt(1).Id,
                    Value = "First Value"
                }
            };

            this.context.DbContext.Projects.Add(project);
            this.context.DbContext.Entities.Add(entity);
            this.context.DbContext.Properties.AddRange(entity.Properties);
            this.context.DbContext.Entries.Add(entry);
            this.context.DbContext.Values.AddRange(entry.Values);

            this.context.DbContext.SaveChanges();

            return project;
        }
    }
}