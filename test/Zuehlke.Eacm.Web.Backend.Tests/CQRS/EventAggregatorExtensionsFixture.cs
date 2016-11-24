using System;
using CQRSlite.Events;
using Xunit;
using Zuehlke.Eacm.Web.Backend.DomainModel;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.Tests.CQRS
{
    public class EventAggregatorExtensionsFixture
    {
        [Fact]
        public void PublishEvent_EventAggregatorIsNull_ThrowsException()
        {
            // arrange
            IEventAggregator eventAggegator = null;
            IEvent e = new TestEvent();

            // act
            Assert.ThrowsAny<ArgumentNullException>(() => eventAggegator.PublishEvent(e));
        }

        [Fact]
        public void PublishEvent_EventIsNull_ThrowsException()
        {
            // arrange
            IEventAggregator eventAggegator = new EventAggregator();
            IEvent e = null;

            // act
            Assert.ThrowsAny<ArgumentNullException>(() => eventAggegator.PublishEvent(e));
        }

        [Fact]
        public void PublishEvent_WithEventSubscription_HandlerGetsCalleds()
        {
            // arrange
            IEventAggregator eventAggegator = new EventAggregator();
            IEvent e = new TestEvent();

            bool executed = false;

            eventAggegator.Subscribe<TestEvent>(ev => executed = true);

            // act
            eventAggegator.PublishEvent(e);

            Assert.True(executed);
        }

        private class TestEvent : IEvent
        {
            public Guid Id { get; set; }

            public int Version { get; set; }

            public DateTimeOffset TimeStamp { get; set; }
        }
    }
}
