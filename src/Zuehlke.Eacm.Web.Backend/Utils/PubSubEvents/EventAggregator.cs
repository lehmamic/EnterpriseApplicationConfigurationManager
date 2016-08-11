using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Zuehlke.Eacm.Web.Backend.Diagnostics;

namespace Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, ICollection<IEventSubscription>> eventSubscriptions = new Dictionary<Type, ICollection<IEventSubscription>>();

        public SubscriptionToken Subscribe<TEvent>(Action<TEvent> action, Predicate<TEvent> filter)
        {
            action.ArgumentNotNull(nameof(action));
            filter.ArgumentNotNull(nameof(filter));

            Type eventType = typeof(TEvent);
            if(!this.eventSubscriptions.ContainsKey(eventType))
            {
                this.eventSubscriptions.Add(eventType, new Collection<IEventSubscription>());
            }

            ICollection<IEventSubscription> subscriptions = this.eventSubscriptions[eventType];
            
            var eventSubscription = new EventSubscription<TEvent>(action, filter);
            eventSubscription.SubscriptionToken = new SubscriptionToken(t => subscriptions.Remove(eventSubscription));

            subscriptions.Add(eventSubscription);

            return eventSubscription.SubscriptionToken;
        }

        public void Unsubscribe(SubscriptionToken token)
        {
            token.ArgumentNotNull(nameof(token));

            var subscription = this.eventSubscriptions.Values
                .SelectMany(c => c)
                .FirstOrDefault(s => s.SubscriptionToken.Equals(token));
            
            if(subscription != null)
            {
                subscription.SubscriptionToken.Dispose();
            }
        }

        public void Publish<TEvent>(TEvent payload)
        {
            var targetEventTypes = this.eventSubscriptions.Keys
                .Where(t => t.IsAssignableFrom(typeof(TEvent)));

            foreach(var subscription in targetEventTypes.SelectMany(t => this.eventSubscriptions[t]))
            {
                Action<object> executionStrategy = subscription.GetExecutionStrategy();
                executionStrategy(payload);
            }
        }
    }
}
