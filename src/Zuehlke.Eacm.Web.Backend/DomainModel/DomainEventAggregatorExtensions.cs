using System;
using System.Reflection;
using CQRSlite.Events;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.DomainModel
{
    public static class DomainEventAggregatorExtensions
    {
        public static void PublishEvent(this IEventAggregator eventAggregator, IEvent e)
        {
            eventAggregator.ArgumentNotNull(nameof(eventAggregator));
            e.ArgumentNotNull(nameof(e));

            MethodInfo publishMethod = typeof(IEventAggregator).GetMethod(nameof(IEventAggregator.Publish));
            if(publishMethod == null)
            {
                throw new InvalidOperationException($"The {nameof(IEventAggregator.Publish)} of the event aggregator could not be found.");
            }

            MethodInfo genericPublishMethod = publishMethod.MakeGenericMethod(e.GetType());
            genericPublishMethod.Invoke(eventAggregator, new object[] { e });
        }
    }
}
