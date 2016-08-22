using System;
using System.Collections.Generic;
using System.Linq;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class ModelDefinition : ModelNode
    {
        private readonly List<EntityDefinition> entities = new List<EntityDefinition>();

        public ModelDefinition(IEventAggregator eventAggregator)
            : base(eventAggregator, Guid.NewGuid())
        {
            this.EventAggregator.Subscribe<EntityDefinitionAdded>(this.OnEntityDefinitionAdded);
            this.EventAggregator.Subscribe<EntityDefinitionDeleted>(this.OnEntityDefinitionDeleted);        
        }

        public IEnumerable<EntityDefinition> Entities => this.entities;

        private void OnEntityDefinitionAdded(EntityDefinitionAdded e)
        {
            var entity = new EntityDefinition(this.EventAggregator, e.EntityId, e.Name, e.Description);
            this.entities.Add(entity);
        }

        private void OnEntityDefinitionDeleted(EntityDefinitionDeleted e)
        {
            EntityDefinition entity = this.entities.FirstOrDefault(i => i.Id == e.EntityId);
            if(entity != null)
            {
                this.entities.Remove(entity);
            }
        }
    }
}
