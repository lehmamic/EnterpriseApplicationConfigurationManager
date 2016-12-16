using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class ModifyPropertyCommand : IDomainCommand
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PropertyType { get; set; }

        public int ExpectedVersion { get; set; }
    }
}