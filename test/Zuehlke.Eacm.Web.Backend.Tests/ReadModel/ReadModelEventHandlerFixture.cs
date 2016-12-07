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

        private void InitializeBasicReadModel()
        {
            var project = new ConfigurationProject
            {
                Id = Guid.NewGuid(),
                Version = 1,
                TimeStamp = DateTimeOffset.Now.AddDays(-1),
                Name = "Initial project name"
            };

            this.context.DbContext.Projects.Add(project);
            this.context.DbContext.SaveChanges();
        }
    }
}