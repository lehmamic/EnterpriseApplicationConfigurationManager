using System;
using Xunit;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.Tests.Utils.PubSubEvents
{
    public class EventSubscriptionFixture
    {
        [Fact]
        public void Constructor_WithActionIsNull_ThrowsException()
        {
            // arrange
            Action<object> action = null;
            Predicate<object> filter = o => true;

            // act
            Assert.ThrowsAny<ArgumentNullException>(() => new EventSubscription<object>(action, filter));
        }

        [Fact]
        public void Constructor_WithFilterIsNull_ThrowsException()
        {
            // arrange
            Action<object> action = o => {};
            Predicate<object> filter = null;

            // act
            Assert.ThrowsAny<ArgumentNullException>(() => new EventSubscription<object>(action, filter));
        }

        [Fact]
        public void GetExecutionStrategy_WithAction_ReturnsNotNull()
        {
            // arrange
            Action<object> action = o => {};
            Predicate<object> filter = o => true;

            var target = new EventSubscription<object>(action, filter);

            // act
            Action<object> actual = target.GetExecutionStrategy();


            //assert
            Assert.NotNull(actual);
        }

        [Fact]
        public void GetExecutionStrategy_WithFilterReturnsTrue_WillExecuteOriginalAction()
        {
            // arrange
            bool executed = false;

            Action<object> action = o => executed = true;
            Predicate<object> filter = o => true;

            var target = new EventSubscription<object>(action, filter);

            // act
            Action<object> actual = target.GetExecutionStrategy();
            actual(new object());

            //assert
            Assert.True(executed);
        }

        [Fact]
        public void GetExecutionStrategy_WithFilterReturnsFalse_WillNotExecuteOriginalAction()
        {
            // arrange
            bool executed = false;

            Action<object> action = o => executed = true;
            Predicate<object> filter = o => false;

            var target = new EventSubscription<object>(action, filter);

            // act
            Action<object> actual = target.GetExecutionStrategy();
            actual(new object());

            //assert
            Assert.False(executed);
        }
    }
}
