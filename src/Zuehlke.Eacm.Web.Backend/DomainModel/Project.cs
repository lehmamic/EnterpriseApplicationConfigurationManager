using System;
using System.Collections.Generic;
using Zuehlke.Eacm.Web.Backend.CQRS;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.DomainModel.Commands;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public class Project : AggregateRoot,
        ICommandHandler<ChangeProjectAttributes>,
        IEventHandler<ProjectAttributesChanged>
    {
        private Dictionary<EntityDefinition, ConfigurationEntity> configurations = new Dictionary<EntityDefinition, ConfigurationEntity>();

        public string Name { get; private set; }

        public string Description { get; private set; }

        public ModelDefinition Definition { get; } = new ModelDefinition();


        public IEnumerable<IEvent> Handle(ChangeProjectAttributes command)
        {
            @command.ArgumentNotNull(nameof(command));

            var e = new ProjectAttributesChanged
            {
                Id = Guid.NewGuid(),
                SourceId = this.Id,
                Name = command.Name,
                Description = command.Description,
            };

            this.Update(e);

        	yield return e;
        }

        public void Handle(ProjectAttributesChanged @event)
        {
            @event.ArgumentNotNull(nameof(@event));

            this.Modified = @event.Timestamp;

         	this.Name = @event.Name;
        	this.Description = @event.Description;
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