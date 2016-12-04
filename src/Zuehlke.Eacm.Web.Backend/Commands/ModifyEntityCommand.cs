using System;
using CQRSlite.Commands;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class ModifyEntityCommand : ICommand
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ExpectedVersion { get; set; }
    }
}