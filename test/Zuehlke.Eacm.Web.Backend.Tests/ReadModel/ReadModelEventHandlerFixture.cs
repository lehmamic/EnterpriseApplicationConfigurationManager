using System;
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

            this.InitializeBasicReadModel();
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
            var initialProject = this.context.DbContext.Projects.First();

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
            var initialProject = this.context.DbContext.Projects.First();

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
            var initialProject = this.context.DbContext.Projects.First();
            var initialEntity = this.context.DbContext.Entities.First();

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

        private void InitializeBasicReadModel()
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
                Name = "Initial entity name",
                Description = "Initial entity description",
                ProjectId = project.Id
            };

            this.context.DbContext.Projects.Add(project);
            this.context.DbContext.Entities.Add(entity);

            this.context.DbContext.SaveChanges();
        }
    }
}