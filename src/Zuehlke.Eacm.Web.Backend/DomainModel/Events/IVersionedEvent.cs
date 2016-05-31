using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public interface IVersionedEvent
    {
        Guid Id { get; }
        
        DateTime Timestamp { get; set; }
        
        Guid CorrelationId { get; set; }
    }
}
