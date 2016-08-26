using System;
using System.Collections.Generic;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class EntityDefinition : ModelNode
    {
        private readonly List<PropertyDefinition> properties = new List<PropertyDefinition>();

        public EntityDefinition (IEventAggregator eventAggregator, Guid id, string name, string description)
            : base(eventAggregator, id)
        {
            this.Name = name;
            this.Description = description;

            this.EventAggregator.Subscribe<EntityDefinitionModified>(this.OnEntityDefinitionModified, e => e.EntityId == this.Id); 
            this.EventAggregator.Subscribe<PropertyDefinitionAdded>(this.OnPropertyDefinitionAdded, e => e.ParentEntityId == this.Id);        
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<PropertyDefinition> Properties => this.properties;

        private void OnEntityDefinitionModified(EntityDefinitionModified e)
        {
            this.Name = e.Name;
            this.Description = e.Description;
        }

        private void OnPropertyDefinitionAdded(PropertyDefinitionAdded e)
        {
            var entity = new PropertyDefinition(this.EventAggregator, e.PropertyId, e.Name, e.Description, e.PropertyType);
            this.properties.Add(entity);
        }
    }
}
