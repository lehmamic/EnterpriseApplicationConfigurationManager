using System;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class PropertyDefinition : ModelNode
    {
        public PropertyDefinition(IEventAggregator eventAggregator, Guid id, string name, string description, string propertyType)
            : base(eventAggregator, id)
        {
            this.Name = name;
            this.Description = description;
            this.PropertyType = propertyType;      
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string PropertyType { get; private set; }

        //// public EntityDefinition Reference { get; private set; }
    }
}
