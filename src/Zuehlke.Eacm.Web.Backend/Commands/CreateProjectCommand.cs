using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class CreateProjectCommand : IDomainCommand
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int ExpectedVersion { get; set; }
    }
}