using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
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
            var response = await this.context.Client.PostAsJsonAsync("api/projects", createProjectDto);

            // assert
            Assert.True(response.IsSuccessStatusCode);

            var projectDto = await response.ReadAsAsync<ProjectDto>();
            Assert.Equal(createProjectDto.Name, projectDto.Name);
        }
    }
}
