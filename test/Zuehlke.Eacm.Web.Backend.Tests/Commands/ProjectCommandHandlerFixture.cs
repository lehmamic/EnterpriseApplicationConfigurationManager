using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
		public async Task Hanlde_CreateProjectCommand_AddsAndCommitsExpectedProject()
        {
            // arrange
            var command = new CreateProjectCommand { Id = Guid.NewGuid(), Name = "AnyProject" };
            var session = new Mock<ISession>();

            var target = new ProjectCommandHandler(session.Object);

            // act
            await target.Handle(command);

            // assert
			session.Verify(s => s.Add(It.Is<Project>(p => p.Id == command.Id && p.Name == command.Name), It.IsAny<CancellationToken>()));
            session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
        }

		[Fact]
		public async Task Hanlde_ModifyProjectAttributesCommand_ModifiesAttributesFromProjectAsync()
		{
			// arrange
			var project = CreateProject();

			var command = new ModifyProjectCommand
			{
				Id = project.Id,
				Name = "NewProjectName",
				Description = "NewProjectDescription"
			};

			var session = new Mock<ISession>();
			session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion, It.IsAny<CancellationToken>())).Returns(Task.FromResult(project));

			var target = new ProjectCommandHandler(session.Object);

			// act
			await target.Handle(command);

			// assert
			Assert.Equal(command.Name, project.Name);
			Assert.Equal(command.Description, project.Description);
			session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
		}

		[Fact]
		public async Task Hanlde_CreateEntityCommand_AddsEntityDefinitionToProjectAsync()
		{
			// arrange
			var project = CreateProject();

			var command = new CreateEntityCommand
			{
				Id = project.Id,
				Name = "NewEntityName",
				Description = "NewEntityDescription"
			};

			var session = new Mock<ISession>();
			session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion, It.IsAny<CancellationToken>())).Returns(Task.FromResult(project));

			var target = new ProjectCommandHandler(session.Object);

			// act
			await target.Handle(command);

			// assert
			var entityDefinition = project.Schema.Entities.FirstOrDefault(e => e.Name == command.Name);

			Assert.NotNull(entityDefinition);
			Assert.Equal(command.Name, entityDefinition.Name);
			Assert.Equal(command.Description, entityDefinition.Description);
			session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
		}

		[Fact]
		public async Task Hanlde_ModifyEntityCommand_ModifiesEntityDefinitionInProjectAsync()
		{
			// arrange
			var project = CreateProject();
			var entityId = project.Schema.Entities.First().Id;

			var command = new ModifyEntityCommand
			{
				Id = project.Id,
				EntityId = entityId,
				Name = "NewEntityName",
				Description = "NewEntityDescription"
			};

			var session = new Mock<ISession>();
			session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion, It.IsAny<CancellationToken>())).Returns(Task.FromResult(project));

			var target = new ProjectCommandHandler(session.Object);

			// act
			await target.Handle(command);

			// assert
			var entityDefinition = project.Schema.Entities.First(e => e.Id == entityId);

			Assert.Equal(command.Name, entityDefinition.Name);
			Assert.Equal(command.Description, entityDefinition.Description);
			session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
		}

		[Fact]
		public async Task Hanlde_DeleteEntityCommand_RemovesEntityDefinitionFromProjectAsync()
		{
			// arrange
			var project = CreateProject();
			var entityId = project.Schema.Entities.First().Id;

			var command = new DeleteEntityCommand
			{
				Id = project.Id,
				EntityId = entityId
			};

			var session = new Mock<ISession>();
			session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion, It.IsAny<CancellationToken>())).Returns(Task.FromResult(project));

			var target = new ProjectCommandHandler(session.Object);

			// act
			await target.Handle(command);

			// assert
			var entityDefinition = project.Schema.Entities.FirstOrDefault(e => e.Id == entityId);

			Assert.Null(entityDefinition);
			session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
		}

		[Fact]
		public async Task Hanlde_CreatePropertyCommand_AddsPropertyDefinitionToProjectAsync()
		{
			// arrange
			var project = CreateProject();
			var entity = project.Schema.Entities.First();

			var command = new CreatePropertyCommand
			{
				Id = project.Id,
				ParentEntityId = entity.Id,
				Name = "NewPropertyName",
				Description = "NewPropertyDescription",
				PropertyType = "Zuehlke.Eacm.String"
			};

			var session = new Mock<ISession>();
			session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion, It.IsAny<CancellationToken>())).Returns(Task.FromResult(project));

			var target = new ProjectCommandHandler(session.Object);

			// act
			await target.Handle(command);

			// assert
			var propertyDefinition = entity.Properties.FirstOrDefault(p => p.Name == command.Name);

			Assert.NotNull(propertyDefinition);
			Assert.Equal(command.Name, propertyDefinition.Name);
			Assert.Equal(command.Description, propertyDefinition.Description);
			Assert.Equal(command.PropertyType, propertyDefinition.PropertyType);
			session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
		}

		[Fact]
		public async Task Hanlde_ModifyPropertyCommand_ModifiesPropertyDefinitionInProjectAsync()
		{
			// arrange
			var project = CreateProject();
			var entity = project.Schema.Entities.First();
			var property = entity.Properties.First();

			var command = new ModifyPropertyCommand
			{
				Id = project.Id,
				PropertyId = property.Id,
				Name = "NewPropertyName",
				Description = "NewPropertyDescription",
				PropertyType = "Zuehlke.Eacm.Integer"
			};

			var session = new Mock<ISession>();
			session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion, It.IsAny<CancellationToken>())).Returns(Task.FromResult(project));

			var target = new ProjectCommandHandler(session.Object);

			// act
			await target.Handle(command);

			// assert
			var propertyDefinition = entity.Properties.FirstOrDefault(p => p.Id == property.Id);

			Assert.Equal(command.Name, propertyDefinition.Name);
			Assert.Equal(command.Description, propertyDefinition.Description);
			Assert.Equal(command.PropertyType, propertyDefinition.PropertyType);
			session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
		}

		[Fact]
		public async Task Hanlde_DeletePropertyCommand_RemovesPropertyDefinitionFromProjectAsync()
		{
			// arrange
			var project = CreateProject();
			var entity = project.Schema.Entities.First();
			var property = entity.Properties.First();

			var command = new DeletePropertyCommand
			{
				Id = project.Id,
				PropertyId = property.Id
			};

			var session = new Mock<ISession>();
			session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion, It.IsAny<CancellationToken>())).Returns(Task.FromResult(project));

			var target = new ProjectCommandHandler(session.Object);

			// act
			await target.Handle(command);

			// assert
			var entityDefinition = entity.Properties.FirstOrDefault(e => e.Id == property.Id);

			Assert.Null(entityDefinition);
			session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
		}

		[Fact]
		public async Task Hanlde_CreateEntryCommand_AddsEntryToProjectAsync()
		{
			// arrange
			var project = CreateProject();
			var entity = project.Schema.Entities.First();

			var command = new CreateEntryCommand
			{
				Id = project.Id,
				EntityId = entity.Id,
				Values = new object[] { "Any Value" }
			};

			var session = new Mock<ISession>();
			session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion, It.IsAny<CancellationToken>())).Returns(Task.FromResult(project));

			var target = new ProjectCommandHandler(session.Object);

			// act
			await target.Handle(command);

			// assert
			var entry = project.Configuration.Entities.First().Entries.Last();

			Assert.NotNull(entry);
			Assert.Equal(command.Values, entry.Values.Select(v => v.Value));
			session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
		}

		[Fact]
		public async Task Hanlde_ModifyEntryCommand_UpdatesEntryValuesInProjectAsync()
		{
			// arrange
			var project = CreateProject();
			var entry = project.Configuration.Entities.First().Entries.First();

			var command = new ModifyEntryCommand
			{
				Id = project.Id,
				EntryId = entry.Id,
				Values = new object[] { "A modifed value" }
			};

			var session = new Mock<ISession>();
			session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion, It.IsAny<CancellationToken>())).Returns(Task.FromResult(project));

			var target = new ProjectCommandHandler(session.Object);

			// act
			await target.Handle(command);

			// assert
			Assert.Equal(command.Values, entry.Values.Select(v => v.Value));
			session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
		}

		[Fact]
		public async Task Hanlde_DeleteEntryCommand_RemovesEntryFromProjectAsync()
		{
			// arrange
			var project = CreateProject();
			var entryId = project.Configuration.Entities.First().Entries.First().Id;

			var command = new DeleteEntryCommand
			{
				Id = project.Id,
				EntryId = entryId
			};

			var session = new Mock<ISession>();
			session.Setup(s => s.Get<Project>(project.Id, command.ExpectedVersion, It.IsAny<CancellationToken>())).Returns(Task.FromResult(project));

			var target = new ProjectCommandHandler(session.Object);

			// act
			await target.Handle(command);

			// assert
			var entry = project.Configuration.Entities.First().Entries.FirstOrDefault(e => e.Id == entryId);

			Assert.Null(entry);
			session.Verify(s => s.Commit(It.IsAny<CancellationToken>()));
		}

		private static Project CreateProject()
        {
            var project = new Project(Guid.NewGuid(), "InitialProjectName");
            project.AddEntityDefinition("Initial Entity", string.Empty);

            var entity = project.Schema.Entities.First();
            project.AddPropertyDefinition(entity.Id, "Initial Property", string.Empty, "Zuehlke.Eacm.String");
            project.AddEntry(entity.Id, new object[] { "Yes it is a value" });

            return project;
        }
    }
}