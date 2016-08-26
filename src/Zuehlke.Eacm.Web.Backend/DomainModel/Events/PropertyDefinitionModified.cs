using System;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class PropertyDefinitionModified : EventBase
    {
        public Guid PropertyId { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public string PropertyType { get; set; }
    }
}
