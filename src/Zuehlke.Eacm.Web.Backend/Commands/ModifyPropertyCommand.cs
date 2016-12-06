using System;
using CQRSlite.Commands;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class ModifyPropertyCommand : ICommand
    {
        public Guid Id { get; set; }

        public Guid ParentEntityId { get; set; }

        public Guid PropertyId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PropertyType { get; set; }

        public int ExpectedVersion { get; set; }
    }
}