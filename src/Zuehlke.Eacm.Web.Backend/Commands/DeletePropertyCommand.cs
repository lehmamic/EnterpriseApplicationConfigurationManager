using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class DeletePropertyCommand : IDomainCommand
    {
        public Guid Id { get; set; }

        public Guid PropertyId { get; set; }

        public int ExpectedVersion { get; set; }
    }
}