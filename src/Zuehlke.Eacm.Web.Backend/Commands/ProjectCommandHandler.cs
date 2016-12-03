using System;
using CQRSlite.Commands;
using CQRSlite.Domain;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class ProjectCommandHandler :
        ICommandHandler<CreateProjectCommand>,
        ICommandHandler<ModifyProjectAttributesCommand>
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

        public void Handle(ModifyProjectAttributesCommand message)
        {
            message.ArgumentNotNull(nameof(message));

            var project = this.session.Get<Project>(message.Id, message.ExpectedVersion);
            project.SetProjectAttributes(message.Name, message.Description);

            this.session.Commit();
        }
    }
}