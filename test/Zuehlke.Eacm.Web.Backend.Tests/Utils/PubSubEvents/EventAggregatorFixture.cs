using System;
using Xunit;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.Tests.Utils.PubSubEvents
{
    public class EventAggregatorFixture
    {
        [Fact]
        public void Subscribe_WithActionIsNull_ThrowsExceptions()
        {
            // arrange
            Action<object> action = null;
            Predicate<object> filter = o => true;

            IEventAggregator target = new EventAggregator();

            // act
            Assert.ThrowsAny<ArgumentNullException>(() => target.Subscribe(action, filter));
        }

        [Fact]
        public void Subscribe_WithFilterIsNull_ThrowsExceptions()
        {
            // arrange
            Action<object> action = o => {};
            Predicate<object> filter = null;

            IEventAggregator target = new EventAggregator();

            // act
            Assert.ThrowsAny<ArgumentNullException>(() => target.Subscribe(action, filter));
        }

        [Fact]
        public void Publish_WithSubscription_WillExecuteAction()
        {
            // arrange
            bool executed = false;

            Action<object> action = o => executed = true;
            Predicate<object> filter = o => true;

            IEventAggregator target = new EventAggregator();
            target.Subscribe(action, filter);

            // act
            target.Publish(new object());

            // assert
            Assert.True(executed);
        }

        [Fact]
        public void Publish_WithFilterReturnsFalse_WillNotExecuteAction()
        {
            // arrange
            bool executed = false;

            Action<object> action = o => executed = true;
            Predicate<object> filter = o => false;

            IEventAggregator target = new EventAggregator();
            target.Subscribe(action, filter);

            // act
            target.Publish(new object());

            // assert
            Assert.False(executed);
        }

        [Fact]
        public void Publish_WithActionUnsubscribed_WillNotExecuteAction()
        {
            // arrange
            bool executed = false;

            Action<object> action = o => executed = true;
            Predicate<object> filter = o => true;

            IEventAggregator target = new EventAggregator();
            SubscriptionToken token = target.Subscribe(action, filter);
            target.Unsubscribe(token);

            // act
            target.Publish(new object());

            // assert
            Assert.False(executed);
        }

        [Fact]
        public void Publish_WithOtherEvent_WillNotExecuteAction()
        {
            // arrange
            bool executed = false;

            Action<int> action = o => executed = true;
            Predicate<int> filter = o => true;

            IEventAggregator target = new EventAggregator();
            SubscriptionToken token = target.Subscribe(action, filter);

            // act
            target.Publish(string.Empty);

            // assert
            Assert.False(executed);
        }

        [Fact]
        public void Publish_WithSubscribedToParentObject_WillExecuteAction()
        {
            // arrange
            bool executed = false;

            Action<ITestEvent> action = o => executed = true;
            Predicate<ITestEvent> filter = o => true;

            IEventAggregator target = new EventAggregator();
            SubscriptionToken token = target.Subscribe(action, filter);

            // act
            target.Publish(new TestEvent());

            // assert
            Assert.True(executed);
        }

        private interface ITestEvent
        {
        }

        private class TestEvent : ITestEvent
        {
        }
    }
}
