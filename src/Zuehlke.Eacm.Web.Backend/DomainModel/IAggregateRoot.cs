using System;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}