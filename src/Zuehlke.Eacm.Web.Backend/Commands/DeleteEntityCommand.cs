using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class DeleteEntityCommand : IDomainCommand
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public int ExpectedVersion { get; set; }
    }
}