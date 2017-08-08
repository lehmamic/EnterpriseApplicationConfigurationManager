using System;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Messages;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class ProjectCommandHandler :
        ICommandHandler<CreateProjectCommand>,
        ICommandHandler<ModifyProjectCommand>,
        ICommandHandler<CreateEntityCommand>,
        ICommandHandler<ModifyEntityCommand>,
        ICommandHandler<DeleteEntityCommand>,
        ICommandHandler<CreatePropertyCommand>,
        ICommandHandler<ModifyPropertyCommand>,
        ICommandHandler<DeletePropertyCommand>,
        ICommandHandler<CreateEntryCommand>,
        ICommandHandler<ModifyEntryCommand>,
        ICommandHandler<DeleteEntryCommand>
    {
        private readonly ISession session;

        public ProjectCommandHandler(ISession session)
        {
            this.session = session.ArgumentNotNull(nameof(session));
        }

        public async Task Handle(CreateProjectCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var item = new Project(message.Id, message.Name);
            await this.session.Add(item);

			await this.session.Commit();
        }

        public async Task Handle(ModifyProjectCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = await this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.SetProjectAttributes(message.Name, message.Description);

			await this.session.Commit();
        }

        public async Task Handle(CreateEntityCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = await this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.AddEntityDefinition(message.Name, message.Description);

            await this.session.Commit();
        }

        public async Task Handle(ModifyEntityCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = await this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.ModifyEntityDefinition(message.EntityId, message.Name, message.Description);

            await this.session.Commit();
        }

        public async Task Handle(DeleteEntityCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = await this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.DeleteEntityDefinition(message.EntityId);

            await this.session.Commit();
        }

        public async Task Handle(CreatePropertyCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = await this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.AddPropertyDefinition(message.ParentEntityId, message.Name, message.Description, message.PropertyType);

			await this.session.Commit();
        }

        public async Task Handle(ModifyPropertyCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = await this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.ModifyPropertyDefinition(message.PropertyId, message.Name, message.Description, message.PropertyType);

            await this.session.Commit();
        }

        public async Task Handle(DeletePropertyCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = await this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.DeletePropertyDefinition(message.PropertyId);

            await this.session.Commit();
        }

        public async Task Handle(CreateEntryCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = await this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.AddEntry(message.EntityId, message.Values);

			await this.session.Commit();
        }

        public async Task Handle(ModifyEntryCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = await this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.ModifyEntry(message.EntryId, message.Values);

            await this.session.Commit();
        }

        public async Task Handle(DeleteEntryCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = await this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.DeleteEntry(message.EntryId);

            await this.session.Commit();
        }
	}
}