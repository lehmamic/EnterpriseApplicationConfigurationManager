using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class CreateEntityCommand : DomainCommand
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}