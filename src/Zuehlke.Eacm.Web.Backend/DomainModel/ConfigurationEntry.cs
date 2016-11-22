using System;
using System.Collections.Generic;
using System.Linq;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class ConfigurationEntry : ModelNode
    {
        private readonly Dictionary<Guid, ConfigurationValue> values;
        
        public ConfigurationEntry(IEventAggregator eventAggregator, EntityDefinition entityDefinition, Guid entryId, IEnumerable<ConfigurationValue> values)
            : base(eventAggregator, entityDefinition.ArgumentNotNull(nameof(entityDefinition)).Id)
        {
            this.Definition = entityDefinition.ArgumentNotNull(nameof(entityDefinition));
            this.values = values.ArgumentNotNull(nameof(values)).ToDictionary(k => k.Property.Id);

            this.EventAggregator.Subscribe<PropertyDefinitionAdded>(this.OnPropertyDefinitionAdded, e => e.ParentEntityId == this.Definition.Id);
            this.EventAggregator.Subscribe<PropertyDefinitionDeleted>(this.OnPropertyDefinitionDeleted);
        }


        public EntityDefinition Definition { get; }

        public ConfigurationValue this[PropertyDefinition property] => this.values[property.ArgumentNotNull(nameof(property)).Id];

        public ConfigurationValue this[Guid propertyId] => this.values[propertyId];

        public IEnumerable<ConfigurationValue> Values => this.values.Values;

        private void OnPropertyDefinitionAdded(PropertyDefinitionAdded e)
        {
            var property = this.Definition.Properties.Single(p => p.Id == e.PropertyId);
            var value = new ConfigurationValue(this.EventAggregator, property, null);

            this.values.Add(property.Id, value);
        }

        private void OnPropertyDefinitionDeleted(PropertyDefinitionDeleted e)
        {
            if (this.values.ContainsKey(e.PropertyId))
            {
                this.values.Remove(e.PropertyId);
            }
        }
    }
}
