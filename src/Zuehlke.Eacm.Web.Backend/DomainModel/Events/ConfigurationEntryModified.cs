using System;
using System.Collections.Generic;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class ConfigurationEntryModified : EventBase
    {
        public Guid EntryId { get; set; }

        public Dictionary<Guid, object> Values { get; set; }
    }
}
