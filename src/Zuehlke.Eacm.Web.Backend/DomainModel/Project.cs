using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class Project
    {
        public ModelDefinition Definition { get; } = new ModelDefinition();


    }

    public class ModelDefinition
    {
        private readonly List<EntityDefinition> entities = new List<EntityDefinition>();
    }

    public class EntityDefinition
    {
        private readonly List<PropertyDefinition> entities = new List<PropertyDefinition>();

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
        
    }

    public class ConfigurationValue
    {
        public Guid Id { get; set; }

        public PropertyDefinition Property { get; set; }

        public string Value { get; set; }
    }
}