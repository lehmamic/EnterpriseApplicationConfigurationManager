namespace Zuehlke.Eacm.Web.Backend.DomainModel.Events
{
    public interface IEventHandler<T>
    {
        void Apply(T e);
    }
}