namespace Zuehlke.Eacm.Web.Backend.CQRS
{
    public interface IEventHandler<T>
        where T : IEvent
    {
        void Handle(T @event); 
    } 
}