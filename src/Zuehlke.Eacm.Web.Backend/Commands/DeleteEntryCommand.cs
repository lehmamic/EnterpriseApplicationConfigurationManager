using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class DeleteEntryCommand : IDomainCommand
    {
        public Guid Id { get; set; }

        public Guid EntryId { get; set; }

        public int ExpectedVersion { get; set; }
    }
}