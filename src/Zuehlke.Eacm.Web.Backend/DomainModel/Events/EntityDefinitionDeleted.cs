using System;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class EntityDefinitionDeleted : EventBase
    {
        public Guid EntityId { get; set; }
    }
}
