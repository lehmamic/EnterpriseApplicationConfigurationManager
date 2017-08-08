using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class DeleteEntryCommand : DomainCommand
    {
        public Guid EntryId { get; set; }
    }
}