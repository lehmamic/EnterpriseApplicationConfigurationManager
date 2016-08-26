using System;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
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

            this.EventAggregator.Subscribe<PropertyDefinitionModified>(this.OnPropertyDefinitionModified, e => e.PropertyId == this.Id);              
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public string PropertyType { get; private set; }

        //// public EntityDefinition Reference { get; private set; }

        private void OnPropertyDefinitionModified(PropertyDefinitionModified e)
        {
            this.Name = e.Name;
            this.Description = e.Description;
            this.PropertyType = e.PropertyType;
        }
    }
}
