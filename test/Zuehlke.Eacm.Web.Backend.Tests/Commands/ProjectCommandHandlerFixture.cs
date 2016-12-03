using System;
using CQRSlite.Domain;
using Moq;
using Xunit;
using Zuehlke.Eacm.Web.Backend.Commands;
using Zuehlke.Eacm.Web.Backend.DomainModel;

namespace Zuehlke.Eacm.Web.Backend.Tests.Commands
{
    public class ProjectCommandHandlerFixture
    {
        [Fact]
        public void Hanlde_CreateProjectCommand_AddsAndCommitsExpectedProject()
        {
            // arrange
            var command = new CreateProjectCommand { Id = Guid.NewGuid(), Name = "AnyProject" };
            var session = new Mock<ISession>();

            var target = new ProjectCommandHandler(session.Object);

            // act
            target.Handle(command);

            // assert
            session.Verify(s => s.Add(It.Is<Project>(p => p.Id == command.Id && p.Name == command.Name)));
            session.Verify(s => s.Commit());
        }

        [Fact]
        public void Hanlde_ModifyProjectAttributesCommand_ModifiesAttributesFromProject()
        {
            // arrange
            var project = CreateProject();

            var command = new ModifyProjectAttributesCommand
            {
                Id = project.Id,
                Name = "NewProjectName",
                Description = "NewProjectDescription"
            };

            var session = new Mock<ISession>();
            session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion)).Returns(project);

            var target = new ProjectCommandHandler(session.Object);

            // act
            target.Handle(command);

            // assert
            Assert.Equal(command.Name, project.Name);
            Assert.Equal(command.Description, project.Description);
            session.Verify(s => s.Commit());
        }

        private static Project CreateProject()
        {
            var project = new Project(Guid.NewGuid(), "InitialProjectName");

            return project;
        }
    }
}