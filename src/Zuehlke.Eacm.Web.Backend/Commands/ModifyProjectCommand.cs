using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class ModifyProjectCommand : DomainCommand
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}