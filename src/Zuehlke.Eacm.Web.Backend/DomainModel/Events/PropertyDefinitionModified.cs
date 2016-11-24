using System;
using CQRSlite.Events;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class PropertyDefinitionModified : IEvent
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public Guid PropertyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PropertyType { get; set; }
    }
}
