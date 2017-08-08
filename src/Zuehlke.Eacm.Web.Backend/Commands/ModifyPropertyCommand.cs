using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class ModifyPropertyCommand : DomainCommand
    {
        public Guid PropertyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PropertyType { get; set; }
    }
}