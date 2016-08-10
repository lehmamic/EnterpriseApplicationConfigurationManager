using Xunit;
using Zuehlke.Eacm.Web.Backend.Utils.PubSubEvents;

namespace Zuehlke.Eacm.Web.Backend.Tests.Utils.PubSubEvents
{
    public class SubscriptionTokenFixture
    {
        [Fact]
        public void Equals_WithSameInstance_ReturnsTrue()
        {
            // arrange
            var token1 = new SubscriptionToken(t => {});

            // act
            var actual = token1.Equals(token1);

            // assert
            Assert.True(actual);
        }

        [Fact]
        public void Equals_WithDifferentInstance_ReturnsFalse()
        {
            // arrange
            var token1 = new SubscriptionToken(t => {});
            var token2 = new SubscriptionToken(t => {});

            // act
            var actual = token1.Equals(token2);

            // assert
            Assert.False(actual);
        }

        public void Dispose_UnsubscriptionActionIsNotNull_ExecutesUnsubscriptionAction()
        {
            // arrange
            var executed = false;
            var token = new SubscriptionToken(t => executed = true );

            // act
            token.Dispose();

            // assert
            Assert.True(executed);
        }
    }
}
