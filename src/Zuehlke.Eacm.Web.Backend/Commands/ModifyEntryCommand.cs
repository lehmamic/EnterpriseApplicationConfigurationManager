using System;
using System.Collections.Generic;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class ModifyEntryCommand : DomainCommand
    {
        public Guid EntryId { get; set; }

        public IEnumerable<object> Values { get; set; }
    }
}