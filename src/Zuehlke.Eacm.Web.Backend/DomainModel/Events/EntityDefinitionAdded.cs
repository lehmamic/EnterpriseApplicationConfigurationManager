using System;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class EntityDefinitionAdded : EventBase
    {
        public Guid EntityId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
