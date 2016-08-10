using System;
using Zuehlke.Eacm.Web.Backend.Diagnostics;

namespace Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents
{
    public class EventSubscription<TPayload> : IEventSubscription
    {
        private readonly Action<TPayload> action;
        private readonly Predicate<TPayload> filter;

        public EventSubscription(Action<TPayload> action, Predicate<TPayload> filter)
        {
            this.action = action.ArgumentNotNull(nameof(action));
            this.filter = filter.ArgumentNotNull(nameof(filter));
        }

        public SubscriptionToken SubscriptionToken { get; set; }

        public Action<object> GetExecutionStrategy()
        {
            Action<TPayload> action = this.action;
            Predicate<TPayload> filter = this.filter;

            return argument =>
            {
                var payload = (TPayload)argument;
                if (filter(payload))
                {
                    this.InvokeAction(action, payload);
                }
            };
        }

        private void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            action.ArgumentNotNull(nameof(action));
            
            action(argument);
        }
    }
}
