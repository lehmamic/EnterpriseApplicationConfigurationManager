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
        protected Project(Guid id)
            : base(id)
        {
            this.EventAggregator.Subscribe<ProjectAttributesChanged>(this.OnProjectAttributesChanged);
        }

        public Project(Guid id, IEnumerable<IEvent> history)
            : this(id)
        {
            history.ArgumentNotNull(nameof(history))
                .ItemsNotNull(nameof(history))
                .ExpectedCondition(i => i.All(e => e.SourceId == id), "The history contains events from another source object.", nameof(history));

            this.Schema = new ModelDefinition(this.EventAggregator);
            this.Configuration = new Configuration(this.EventAggregator, this.Schema); 

            this.LoadFrom(history);
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public ModelDefinition Schema { get; }

        public Configuration Configuration { get; }

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

            var entity = this.GetEntityDefinition(entityId);

            var e = new EntityDefinitionModified
            {
                EntityId = entity.Id,
                Name = name,
                Description = description,
            };

            this.Update(e);
        }

        public void DeleteEntityDefinition(Guid entityId)
        {
            var e = new EntityDefinitionDeleted
            {
                EntityId = entityId
            };

            this.Update(e);
        }

        public void AddPropertyDefinition(Guid entityId, string name, string description, string propertyType)
        {
            name.ArgumentNotNullOrEmpty(nameof(name));
            description.ArgumentNotNull(nameof(description));
            propertyType.ArgumentNotNullOrEmpty(nameof(propertyType));

            // the entity gets fetched to assert its existence.
            EntityDefinition entity = this.GetEntityDefinition(entityId);

            var e = new PropertyDefinitionAdded
            {
                ParentEntityId = entity.Id,
                PropertyId = Guid.NewGuid(),
                Name = name,
                Description = description,
                PropertyType = propertyType
            };

            this.Update(e);
        }

        public void ModifyPropertyDefinition(Guid propertyId, string name, string description, string propertyType)
        {
            name.ArgumentNotNullOrEmpty(nameof(name));
            description.ArgumentNotNull(nameof(description));
            propertyType.ArgumentNotNullOrEmpty(nameof(propertyType));

            // the property gets fetched to assert its existence.
            PropertyDefinition property = this.GetPropertyDefinition(propertyId);

            var e = new PropertyDefinitionModified
            {
                PropertyId = property.Id,
                Name = name,
                Description = description,
                PropertyType = propertyType
            };

            this.Update(e);
        }

        public void DeletePropertyDefinition(Guid propertyId)
        {
            var e = new PropertyDefinitionDeleted
            {
                PropertyId = propertyId
            };

            this.Update(e);
        }

        public void AddEntry(Guid entityId, IEnumerable<object> values)
        {
            var entity = this.Schema.Entities.SingleOrDefault(i => i.Id == entityId);
            if(entity == null)
            {
                throw new ArgumentException($"No entity definition with id {entityId} was found.", nameof(entityId));
            }

            if(entity.Properties.Count() != values.Count())
            {
                throw new ArgumentException("The amount of values does not match the amount of properties.", nameof(values));
            }

            var propertyValues = entity.Properties.Select((p, i) => new { PropertyId = p.Id, Value = values.ElementAt(i) })
                .ToDictionary(i => i.PropertyId, i => i.Value);

            var e = new ConfigurationEntryAdded
            {
                EntityId = entityId,
                EntryId = Guid.NewGuid(),
                Values = propertyValues
            };

            this.Update(e);
        }

        public void DeleteEntry(Guid entryId)
        {
            var e = new ConfigurationEntryDeleted
            {
                EntryId = entryId
            };

            this.Update(e);
        }

        private EntityDefinition GetEntityDefinition(Guid entityId)
        {
            EntityDefinition entity = this.Schema.Entities.SingleOrDefault(i => i.Id == entityId);
            if(entity == null)
            {
                throw new ArgumentException($"No entity definition with id {entityId} was found.", nameof(entityId));
            }

            return entity;
        }

        private PropertyDefinition GetPropertyDefinition(Guid propertyId)
        {
            PropertyDefinition property = this.Schema.Entities
                .SelectMany(e => e.Properties)
                .SingleOrDefault(i => i.Id == propertyId);

            if(property == null)
            {
                throw new ArgumentException($"No property definition with id {propertyId} was found.", nameof(propertyId));
            }

            return property;
        }

        private ConfigurationEntry GetEntry(Guid entryId)
        {
            ConfigurationEntry entry = this.Configuration.Entities
                .SelectMany(e => e.Entries)
                .SingleOrDefault(i => i.Id == entryId);

            if(entry == null)
            {
                throw new ArgumentException($"No configuration entry with id {entryId} was found.", nameof(entryId));
            }

            return entry;
        }

        private void OnProjectAttributesChanged(ProjectAttributesChanged e)
        {
            this.Name = e.Name;
            this.Description = e.Description;
        }
    }
}