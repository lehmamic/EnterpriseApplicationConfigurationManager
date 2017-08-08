using System;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class DeletePropertyCommand : DomainCommand
    {
        public Guid PropertyId { get; set; }
    }
}