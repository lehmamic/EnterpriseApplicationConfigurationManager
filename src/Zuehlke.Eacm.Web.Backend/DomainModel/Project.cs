using System;
using System.Collections.Generic;
using System.Linq;
using CQRSlite.Domain;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class Project : AggregateRoot
    {
        private readonly IEventAggregator eventAggregator = new EventAggregator();

        public Project()
        {
        }

        public Project(Guid id, string name)
        {
            this.Id = id;
            this.Schema = new ModelDefinition(this.eventAggregator);
            this.Configuration = new Configuration(this.eventAggregator, this.Schema);

            var e = new ProjectCreated { Id = id, Name = name };
            this.ApplyChange(e);
        }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public ModelDefinition Schema { get; }

        public Configuration Configuration { get; }

        public void SetProjectAttributes(string name, string description)
        {
            name.ArgumentNotNullOrEmpty(nameof(name));
            description.ArgumentNotNull(nameof(description));

            var e = new ProjectModified
            {
                Id = this.Id,
                Name = name,
                Description = description,
            };

            this.ApplyChange(e);
        }

        public void AddEntityDefinition(string name, string description)
        {
            name.ArgumentNotNullOrEmpty(nameof(name));
            description.ArgumentNotNull(nameof(description));

            var e = new EntityDefinitionAdded
            {
                Id = this.Id,
                EntityId = Guid.NewGuid(),
                Name = name,
                Description = description,
            };

            this.ApplyChange(e);
        }

        public void ModifyEntityDefinition(Guid entityId, string name, string description)
        {
            name.ArgumentNotNullOrEmpty(nameof(name));
            description.ArgumentNotNull(nameof(description));

            var entity = this.GetEntityDefinition(entityId);

            var e = new EntityDefinitionModified
            {
                Id = this.Id,
                EntityId = entity.Id,
                Name = name,
                Description = description,
            };

            this.ApplyChange(e);
        }

        public void DeleteEntityDefinition(Guid entityId)
        {
            var e = new EntityDefinitionDeleted
            {
                Id = this.Id,
                EntityId = entityId
            };

            this.ApplyChange(e);
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
                Id = this.Id,
                ParentEntityId = entity.Id,
                PropertyId = Guid.NewGuid(),
                Name = name,
                Description = description,
                PropertyType = propertyType
            };

            this.ApplyChange(e);
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
                Id = Id,
                PropertyId = property.Id,
                Name = name,
                Description = description,
                PropertyType = propertyType
            };

            this.ApplyChange(e);
        }

        public void DeletePropertyDefinition(Guid propertyId)
        {
            var e = new PropertyDefinitionDeleted
            {
                Id = this.Id,
                PropertyId = propertyId
            };

            this.ApplyChange(e);
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
                Id = this.Id,
                EntityId = entityId,
                EntryId = Guid.NewGuid(),
                Values = propertyValues
            };

            this.ApplyChange(e);
        }

        public void ModifyEntry(Guid entryId, IEnumerable<object> values)
        {
            values.ArgumentNotNull(nameof(values));

            var entry = this.GetEntry(entryId);

            if(entry.Definition.Properties.Count() != values.Count())
            {
                throw new ArgumentException("The amount of values does not match the amount of properties.", nameof(values));
            }

            var propertyValues = entry.Definition.Properties.Select((p, i) => new { PropertyId = p.Id, Value = values.ElementAt(i) })
                .ToDictionary(i => i.PropertyId, i => i.Value);

            var e = new ConfigurationEntryModified
            {
                Id = this.Id,
                EntryId = entryId,
                Values = propertyValues
            };

            this.ApplyChange(e);
        }

        public void DeleteEntry(Guid entryId)
        {
            var e = new ConfigurationEntryDeleted
            {
                Id = this.Id,
                EntryId = entryId
            };

            this.ApplyChange(e);
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

        private void Apply(ProjectCreated e)
        {
            this.Id = e.Id;
            this.Name = e.Name;
        }

        private void Apply(ProjectModified e)
        {
            this.Name = e.Name;
            this.Description = e.Description;
        }

        private void Apply(EntityDefinitionAdded e)
        {
            this.eventAggregator.Publish(e);
        }

        private void Apply(EntityDefinitionModified e)
        {
            this.eventAggregator.Publish(e);
        }

        private void Apply(EntityDefinitionDeleted e)
        {
            this.eventAggregator.Publish(e);
        }

        private void Apply(PropertyDefinitionAdded e)
        {
            this.eventAggregator.Publish(e);
        }

        private void Apply(PropertyDefinitionModified e)
        {
            this.eventAggregator.Publish(e);
        }

        private void Apply(PropertyDefinitionDeleted e)
        {
            this.eventAggregator.Publish(e);
        }

        private void Apply(ConfigurationEntryAdded e)
        {
            this.eventAggregator.Publish(e);
        }

        private void Apply(ConfigurationEntryModified e)
        {
            this.eventAggregator.Publish(e);
        }

        private void Apply(ConfigurationEntryDeleted e)
        {
            this.eventAggregator.Publish(e);
        }
    }
}