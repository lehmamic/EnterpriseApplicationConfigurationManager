using System;

namespace Zuehlke.Eacm.Web.Backend.CQRS
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}