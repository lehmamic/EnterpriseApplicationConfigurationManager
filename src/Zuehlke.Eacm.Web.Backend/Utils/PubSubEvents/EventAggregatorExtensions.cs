using System;
using Zuehlke.Eacm.Web.Backend.Diagnostics;

namespace Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents
{
    public static class EventAggregatorExtensions
    {
        public static SubscriptionToken Subscribe<TEvent>(this IEventAggregator eventAggregator, Action<TEvent> action)
        {
            eventAggregator.ArgumentNotNull(nameof(eventAggregator));
            action.ArgumentNotNull(nameof(action));

            return eventAggregator.Subscribe(action, e => true);
        }
    }
}
