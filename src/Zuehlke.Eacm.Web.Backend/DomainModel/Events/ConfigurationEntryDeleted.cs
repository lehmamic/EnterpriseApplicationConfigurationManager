using System;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class ConfigurationEntryDeleted : EventBase
    {
        public Guid EntryId { get; set; }
    }
}
