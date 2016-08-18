using System;
using System.Collections.Generic;
using System.Linq;
using Zuehlke.Eacm.Web.Backend.CQRS;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class Project : EventSourced
    {
        private readonly Dictionary<EntityDefinition, ConfigurationEntity> configurations = new Dictionary<EntityDefinition, ConfigurationEntity>();

        protected Project(Guid id)
            : base(id)
        {
            this.EventAggregator.Subscribe<ProjectAttributesChanged>(this.OnProjectAttributesChanged);
            this.EventAggregator.Subscribe<EntityDefinitionAdded>(this.OnEntityDefinitionAdded);
            this.EventAggregator.Subscribe<EntityDefinitionModified>(this.OnEntityDefinitionModified);
        }

        public Project(Guid id, IEnumerable<IEvent> history)
            : this(id)
        {
            history.ArgumentNotNull(nameof(history))
                .ItemsNotNull(nameof(history))
                .ExpectedCondition(i => i.All(e => e.SourceId == id), "The history contains events from another source object.", nameof(history));

            this.LoadFrom(history);
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public ModelDefinition Schema { get; } = new ModelDefinition();

        public void SetProjectAttributes(string name, string description)
        {
            name.ArgumentNotNullOrEmpty(nameof(name));
            description.ArgumentNotNull(nameof(description));

            var e = new ProjectAttributesChanged
            {
                Name = name,
                Description = description,
            };

            this.Update(e);
        }

        public void AddEntityDefinition(string name, string description)
        {
            name.ArgumentNotNullOrEmpty(nameof(name));
            description.ArgumentNotNull(nameof(description));

            var e = new EntityDefinitionAdded
            {
                EntityId = Guid.NewGuid(),
                Name = name,
                Description = description,
            };

            this.Update(e);
        }

        public void ModifyEntityDefinition(Guid entityId, string name, string description)
        {
            name.ArgumentNotNullOrEmpty(nameof(name));
            description.ArgumentNotNull(nameof(description));

            var entity = this.Schema.Entities.SingleOrDefault(i => i.Id == entityId);
            if(entity == null)
            {
                throw new ArgumentException($"No entity definition with id {entityId} was found.", nameof(entityId));
            }

            var e = new EntityDefinitionModified
            {
                EntityId = entity.Id,
                Name = name,
                Description = description,
            };

            this.Update(e);
        }

        private void OnProjectAttributesChanged(ProjectAttributesChanged e)
        {
            this.Name = e.Name;
            this.Description = e.Description;
        }

        private void OnEntityDefinitionAdded(EntityDefinitionAdded e)
        {
            throw new NotImplementedException();
        }

        private void OnEntityDefinitionModified(EntityDefinitionModified e)
        {
            throw new NotImplementedException();
        }
    }

    public class ModelDefinition
    {
        private readonly List<EntityDefinition> entities = new List<EntityDefinition>();

        public IEnumerable<EntityDefinition> Entities => this.entities;
    }

    public class EntityDefinition
    {
        private readonly List<PropertyDefinition> entities = new List<PropertyDefinition>();

        private Guid id;
        private string name;

        public Guid Id => this.id;
    }

    public class PropertyDefinition
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string PropertyType { get; set; }

        public EntityDefinition Reference { get; set; }
    }

    public class ConfigurationEntity
    {
    }

    public class ConfigurationEntry
    {
        private Dictionary<PropertyDefinition, string> values = new Dictionary<PropertyDefinition, string>();
    }
}