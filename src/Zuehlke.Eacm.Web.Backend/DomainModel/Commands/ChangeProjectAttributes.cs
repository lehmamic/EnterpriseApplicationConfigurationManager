using Zuehlke.Eacm.Web.Backend.CQRS;

namespace Zuehlke.Eacm.Web.Backend.DomainModel.Commands
{
    public class ChangeProjectAttributes : ICommand
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
