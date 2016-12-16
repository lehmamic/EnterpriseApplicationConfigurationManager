using CQRSlite.Commands;
using CQRSlite.Domain;
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

        public void Handle(CreateProjectCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var item = new Project(message.Id, message.Name);
            this.session.Add(item);

            this.session.Commit();
        }

        public void Handle(ModifyProjectCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.SetProjectAttributes(message.Name, message.Description);

            this.session.Commit();
        }

        public void Handle(CreateEntityCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.AddEntityDefinition(message.Name, message.Description);

            this.session.Commit();
        }

        public void Handle(ModifyEntityCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.ModifyEntityDefinition(message.EntityId, message.Name, message.Description);

            this.session.Commit();
        }

        public void Handle(DeleteEntityCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.DeleteEntityDefinition(message.EntityId);

            this.session.Commit();
        }

        public void Handle(CreatePropertyCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.AddPropertyDefinition(message.ParentEntityId, message.Name, message.Description, message.PropertyType);

            this.session.Commit();
        }

        public void Handle(ModifyPropertyCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.ModifyPropertyDefinition(message.PropertyId, message.Name, message.Description, message.PropertyType);

            this.session.Commit();
        }

        public void Handle(DeletePropertyCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.DeletePropertyDefinition(message.PropertyId);

            this.session.Commit();
        }

        public void Handle(CreateEntryCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.AddEntry(message.EntityId, message.Values);

            this.session.Commit();
        }

        public void Handle(ModifyEntryCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.ModifyEntry(message.EntryId, message.Values);

            this.session.Commit();
        }

        public void Handle(DeleteEntryCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.DeleteEntry(message.EntryId);

            this.session.Commit();
        }
    }
}