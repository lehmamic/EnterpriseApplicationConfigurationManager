using System;
using System.Collections.Generic;
using CQRSlite.Events;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class ConfigurationEntryModified : IEvent
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public Guid EntryId { get; set; }

        public Dictionary<Guid, object> Values { get; set; }
    }
}
