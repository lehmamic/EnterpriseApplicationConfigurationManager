using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class DeleteEntityCommand : DomainCommand
    {
        public Guid EntityId { get; set; }
    }
}