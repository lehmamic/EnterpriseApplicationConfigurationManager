using System;
using System.Collections.Generic;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class CreateEntryCommand : IDomainCommand
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public IEnumerable<object> Values { get; set; }

        public int ExpectedVersion { get; set; }
    }
}