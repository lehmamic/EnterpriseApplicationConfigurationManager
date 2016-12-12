using System.Net.Http;
using System.Text;
using NuGet.Protocol.Core.v3;
using Xunit;
using Zuehlke.Eacm.Web.Backend.Commands;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.EndToEndTests.FixtureSupport;
using Zuehlke.Eacm.Web.Backend.Models;

namespace Zuehlke.Eacm.Web.Backend.EndToEndTests
{
    public class ProjectsApiFixture : IClassFixture<WebHostFixture<Startup>>
    {
        private readonly WebHostFixture<Startup> context;

        public ProjectsApiFixture(WebHostFixture<Startup> context)
        {
            this.context = context.ArgumentNotNull(nameof(context));
        }

        [Fact]
        public async void CreateProject_WithValidProjectName_CreatesProject()
        {
            // arrange
            var createProjectDto = new CreateProjectCommand
            {
                Name = "My Fancy project"
            };

            // act
            var content = createProjectDto.ToJson();
            var response = await this.context.Client.PostAsync("api/projects", new StringContent(content, Encoding.UTF8, "application/json"));

            // assert
            Assert.True(response.IsSuccessStatusCode);

            var projectDto = (await response.Content.ReadAsStringAsync()).FromJson<ProjectDto>();
            Assert.Equal(createProjectDto.Name, projectDto.Name);
        }
    }
}
