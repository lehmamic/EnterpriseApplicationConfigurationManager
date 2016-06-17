using System;
using System.Collections.Generic;
using System.Linq;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class Project : EventSourced
    {
        private Dictionary<EntityDefinition, ConfigurationEntity> configurations = new Dictionary<EntityDefinition, ConfigurationEntity>();

        protected Project(Guid id)
            : base(id)
        {
            base.Handles<ProjectAttributesChanged>(this.OnProjectAttributesChanged);
        }

        public Project(Guid id, IEnumerable<IEvent> history)
            : this(id)
        {
            history.ArgumentNotNull(nameof(history));

            this.LoadFrom(history);
        }

        #region Implementation of IAggregateRoot
        public Guid Id { get; }
        #endregion

        public string Name { get; private set; }

        public string Description { get; private set; }

        public ModelDefinition Definition { get; } = new ModelDefinition();

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

        private void OnProjectAttributesChanged(ProjectAttributesChanged e)
        {
                this.Name = e.Name;
                this.Description = e.Description;
        }
    }

    public class ModelDefinition
    {
        private readonly List<EntityDefinition> entities = new List<EntityDefinition>();
    }

    public class EntityDefinition
    {
        private readonly List<PropertyDefinition> entities = new List<PropertyDefinition>();

        private Guid id;
        private string name;
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