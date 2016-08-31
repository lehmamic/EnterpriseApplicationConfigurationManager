using System;
using System.Collections.Generic;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class Configuration : ModelNode
    {
        private readonly ModelDefinition schema;
        private readonly Dictionary<Guid, ConfigurationEntity> entities = new Dictionary<Guid, ConfigurationEntity>();
        
        public Configuration(IEventAggregator eventAggregator, ModelDefinition schema)
            : base(eventAggregator, Guid.NewGuid())
        {
            this.schema = schema.ArgumentNotNull(nameof(schema));

            this.EventAggregator.Subscribe<EntityDefinitionAdded>(this.OnEntityDefinitionAdded);
            this.EventAggregator.Subscribe<EntityDefinitionDeleted>(this.OnEntityDefinitionDeleted); 
        }

        public ConfigurationEntity this[Guid entityDefinitionId] => this.entities[entityDefinitionId];

        public IEnumerable<ConfigurationEntity> Entities => this.entities.Values;

        private void OnEntityDefinitionAdded(EntityDefinitionAdded e)
        {
            this.entities.Add(e.EntityId, new ConfigurationEntity(this.EventAggregator, e.EntityId, this.schema));
        }

        private void OnEntityDefinitionDeleted(EntityDefinitionDeleted e)
        {
            if (this.entities.ContainsKey(e.EntityId))
            {
                this.entities.Remove(e.EntityId);
            }
        }
    }
}
