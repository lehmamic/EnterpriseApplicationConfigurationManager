using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class ModifyEntityCommand : DomainCommand
    {
        public Guid EntityId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}