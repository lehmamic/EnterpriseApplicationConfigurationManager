using System.Collections.Generic;

namespace Zuehlke.Eacm.Web.Backend.CQRS
{
    public interface ICommandHandler<T> where T: ICommand
    {
        IEnumerable<IEvent> Handle(T command);
    }
}
