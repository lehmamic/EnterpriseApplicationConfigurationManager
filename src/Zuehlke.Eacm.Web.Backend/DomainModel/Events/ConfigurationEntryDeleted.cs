using System;
using CQRSlite.Events;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class ConfigurationEntryDeleted : IEvent
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public Guid EntryId { get; set; }
    }
}
