using System;

namespace Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents
{
    public interface IEventSubscription
    {
        SubscriptionToken SubscriptionToken { get; set; }

        Action<object[]> GetExecutionStrategy();
    }
}
