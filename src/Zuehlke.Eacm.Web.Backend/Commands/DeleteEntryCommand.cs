using System;
using CQRSlite.Commands;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class DeleteEntryCommand : ICommand
    {
        public Guid Id { get; set; }

        public Guid EntryId { get; set; }

        public int ExpectedVersion { get; set; }
    }
}