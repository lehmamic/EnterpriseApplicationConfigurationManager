using System;
using System.Collections.Generic;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class CreateEntryCommand : DomainCommand
    {
        public Guid EntityId { get; set; }

        public IEnumerable<object> Values { get; set; }
    }
}