using System;
using System.Collections.Generic;
using System.Linq;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class ConfigurationEntity : ModelNode
    {
        private readonly ModelDefinition schema;
        private readonly List<ConfigurationEntry> entries = new List<ConfigurationEntry>();

        public ConfigurationEntity(IEventAggregator eventAggregator, Guid id, ModelDefinition schema)
            : base(eventAggregator, id)
        {
            this.schema = schema.ArgumentNotNull(nameof(schema));
            this.Definition = this.schema.Entities.Single(e => e.Id == this.Id);

            this.EventAggregator.Subscribe<ConfigurationEntryAdded>(this.OnConfigurationEntryAdded, e => e.EntityId == this.Definition.Id);
            this.EventAggregator.Subscribe<ConfigurationEntryDeleted>(this.OnConfigurationEntryDeleted, e => this.entries.Any(entry => entry.Id == e.EntryId));
        }

        public EntityDefinition Definition  { get; }

        public IEnumerable<ConfigurationEntry> Entries => this.entries;

        private void OnConfigurationEntryAdded(ConfigurationEntryAdded e)
        {
            var values = e.Values.Select(i => this.CreateConfigurationValue(i.Key, i.Value));
            var entry = new ConfigurationEntry(this.EventAggregator, this.Definition, e.EntryId, values);

            this.entries.Add(entry);
        }

        private void OnConfigurationEntryDeleted(ConfigurationEntryDeleted e)
        {
            var entry = this.entries.SingleOrDefault(item => item.Id == e.EntryId);
            if (entry != null)
            {
                this.entries.Remove(entry);
            }
        }

        private ConfigurationValue CreateConfigurationValue(Guid propertyId, object value)
        {
            return new ConfigurationValue(this.EventAggregator, this.Definition.Properties.Single(p => p.Id == propertyId), value);
        }
    }
}
