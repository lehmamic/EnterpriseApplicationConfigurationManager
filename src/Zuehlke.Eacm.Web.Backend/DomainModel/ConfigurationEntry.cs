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
            : base(eventAggregator, entryId)
        {
            this.Definition = entityDefinition.ArgumentNotNull(nameof(entityDefinition));
            this.values = values.ArgumentNotNull(nameof(values)).ToDictionary(k => k.Property.Id);

            this.EventAggregator.Subscribe<PropertyDefinitionAdded>(this.OnPropertyDefinitionAdded, e => e.ParentEntityId == this.Definition.Id);
            this.EventAggregator.Subscribe<PropertyDefinitionDeleted>(this.OnPropertyDefinitionDeleted);
            this.EventAggregator.Subscribe<ConfigurationEntryModified>(this.OnConfigurationEntryModified, e => e.EntryId == this.Id);
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

        private void OnConfigurationEntryModified(ConfigurationEntryModified e)
        {
            foreach (var propertyId in e.Values.Keys)
            {
                this.values[propertyId] = CreateConfigurationValue(propertyId, e.Values[propertyId]);
            }
        }

        private ConfigurationValue CreateConfigurationValue(Guid propertyId, object value)
        {
            return new ConfigurationValue(this.EventAggregator, this.Definition.Properties.Single(p => p.Id == propertyId), value);
        }
    }
}
