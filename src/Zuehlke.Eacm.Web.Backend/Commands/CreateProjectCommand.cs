using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class CreateProjectCommand : DomainCommand
    {
        public string Name { get; set; }
    }
}