using System;
using CQRSlite.Commands;

namespace Zuehlke.Eacm.Web.Backend.Commands
{
	public abstract class DomainCommand : ICommand
    {
        public Guid Id { get; set; }

		public int ExpectedVersion { get; set; }
	}
}
