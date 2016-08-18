using System;
using Xunit;
using Zuehlke.Eacm.Web.Backend.CQRS;

namespace Zuehlke.Eacm.Web.Backend.Tests.DomainModel
{
    public class EventAggregatorExtensionsFixture
    {
        [Fact]
        public void PublishEvent_()
        {
            // arrange 

            // act

            // assert
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
