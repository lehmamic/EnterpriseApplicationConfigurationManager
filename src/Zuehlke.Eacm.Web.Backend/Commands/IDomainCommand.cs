using System;
using CQRSlite.Commands;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public interface IDomainCommand : ICommand
    {
        Guid Id { get; set; }
    }
}
