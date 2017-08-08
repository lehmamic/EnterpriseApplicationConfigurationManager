using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class CreatePropertyCommand : DomainCommand
    {
        public Guid ParentEntityId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PropertyType { get; set; }
    }
}