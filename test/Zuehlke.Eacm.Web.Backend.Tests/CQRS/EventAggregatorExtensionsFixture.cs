using System;
using Xunit;
using Zuehlke.Eacm.Web.Backend.CQRS;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.Tests.DomainModel
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
            Assert.ThrowsAny<ArgumentNullException>(() => CQRS.EventAggregatorExtensions.PublishEvent(eventAggegator, e));
        }

        [Fact]
        public void PublishEvent_EventIsNull_ThrowsException()
        {
            // arrange
            IEventAggregator eventAggegator = new EventAggregator();
            IEvent e = null;

            // act
            Assert.ThrowsAny<ArgumentNullException>(() => CQRS.EventAggregatorExtensions.PublishEvent(eventAggegator, e));
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
            CQRS.EventAggregatorExtensions.PublishEvent(eventAggegator, e);

            Assert.True(executed);
        }

        private class TestEvent : IEvent
        {
            public Guid CorrelationId
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public Guid Id
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public Guid SourceId
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public DateTime Timestamp
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
