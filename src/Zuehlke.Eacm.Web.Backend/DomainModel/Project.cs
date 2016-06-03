using System;
using System.Collections.Generic;
using System.Linq;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class Project : IAggregateRoot
    {
        private string name;

        private string description;

        private Dictionary<EntityDefinition, ConfigurationEntity> configurations = new Dictionary<EntityDefinition, ConfigurationEntity>();

        public Project(IEnumerable<IEvent> history)
        {
            
        }

        #region Implementation of IAggregateRoot
        public Guid Id { get; }
        #endregion

        public ModelDefinition Definition { get; } = new ModelDefinition();
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