using System;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class PropertyDefinitionAdded : EventBase
    {
        public Guid EntityId { get; set; }

        public Guid PropertyId { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public string PropertyType { get; set; }
    }
}
