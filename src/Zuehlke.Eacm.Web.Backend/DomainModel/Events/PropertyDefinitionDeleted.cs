using System;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public class PropertyDefinitionDeleted : EventBase
    {
        public Guid PropertyId { get; set; }
    }
}
