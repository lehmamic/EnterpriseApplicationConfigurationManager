using System;
using CQRSlite.Commands;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class DeleteEntityCommand : ICommand
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public int ExpectedVersion { get; set; }
    }
}