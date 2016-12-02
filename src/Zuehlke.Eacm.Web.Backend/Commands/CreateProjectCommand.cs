using System;
using CQRSlite.Commands;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
    public class CreateProjectCommand : ICommand
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int ExpectedVersion { get; set; }
    }
}