using System;

namespace Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents
{
    public interface IEventAggregator
    {
        SubscriptionToken Subscribe<TEvent>(Action<TEvent> action, Predicate<TEvent> filter);

        void Unsubscribe(SubscriptionToken token);

        void Publish<TEvent>(TEvent payload);
    }
}