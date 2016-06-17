using System;
using System.Collections.Generic;
using Xunit;
using Zuehlke.Eacm.Web.Backend.DomainModel;
using Zuehlke.Eacm.Web.Backend.DomainModel.Events;

namespace Zuehlke.Eacm.Web.Backend.Tests.DomainModel
{
    public class ProjectFixture
    {
        [Fact]
        public void Constructor_WithEmptyHistory_CreatedDateNotInitialized()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            // act
            var target = new Project(id, history);

            // assert
            Assert.Equal(DateTime.MinValue, target.Created);
        }

        [Fact]
        public void Constructor_WithEmptyHistory_ModifiedDateNotInitialized()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            // act
            var target = new Project(id, history);

            // assert
            Assert.Null(target.Modified);
        }

        [Fact]
        public void Constructor_WithEmptyHistory_NameNotInitialized()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            // act
            var target = new Project(id, history);

            // assert
            Assert.Null(target.Name);
        }

        [Fact]
        public void Constructor_WithEmptyHistory_DescritionNotInitialized()
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            // act
            var target = new Project(id, history);

            // assert
            Assert.Null(target.Description);
        }

        [Theory]
        [InlineData("any name", "any description")]
        [InlineData("any name", "")]
        public void SetProjectAttribute_WithValidParameters_SetsAttributes(string name, string description)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);

            // act
            target.SetProjectAttributes(name, description);

            // assert
            Assert.Equal(name, target.Name);
            Assert.Equal(description, target.Description);
        }

        [Theory]
        [InlineData(null, "any description")]
        [InlineData("", "any description")]
        [InlineData("any name", null)]
        public void SetProjectAttribute_WithInvalidParameters_ThrowsException(string name, string description)
        {
            // arrange
            var id = Guid.NewGuid();
            IEnumerable<IEvent> history = new List<IEvent>();

            var target = new Project(id, history);

            // act
            target.SetProjectAttributes(name, description);

            // assert
            Assert.Equal(name, target.Name);
            Assert.Equal(description, target.Description);
        }
    }
}
