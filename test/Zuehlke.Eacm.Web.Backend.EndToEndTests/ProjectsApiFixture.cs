using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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

        [Fact]
        public async void GetProject_WithValidProjectId_ReturnsProject()
        {
            // arrange
            var expectedProject = await this.PrepareProject();

            // act
            HttpResponseMessage response = await this.context.Client.GetAsync($"api/projects/{expectedProject.Id}");

            // assert
            Assert.True(response.IsSuccessStatusCode);

            var actualProject = await response.ReadAsAsync<ProjectDto>();
            Assert.Equal(expectedProject.Id, actualProject.Id);
            Assert.Equal(expectedProject.Name, actualProject.Name);
        }

        [Fact]
        public async void GetProject_WithoutQueryOptions_ReturnsProjects()
        {
            // arrange
            var expectedProject1 = await this.PrepareProject();
            var expectedProject2 = await this.PrepareProject();

            // act
            HttpResponseMessage response = await this.context.Client.GetAsync("api/projects");

            // assert
            Assert.True(response.IsSuccessStatusCode);

            var actualProjects = (await response.ReadAsAsync<IEnumerable<ProjectDto>>()).ToArray();
            Assert.Contains(actualProjects, p => p.Id == expectedProject1.Id);
            Assert.Contains(actualProjects, p => p.Id == expectedProject2.Id);
        }

        [Fact]
        public async void UpdateProject_WithValidProjectNameAndDescription_UpdatesProjects()
        {
            // arrange
            var existingProject = await this.PrepareProject();

            var value = new ProjectDto
            {
                Name = "Modifed Project Name",
                Description = "Modified Project Description"
            };

            // act
            HttpResponseMessage response = await this.context.Client.PutAsJsonAsync($"api/projects/{existingProject.Id}", value);

            // assert
            Assert.True(response.IsSuccessStatusCode);

            var actualProject = await this.context.Client.ReadAsAsync<ProjectDto>($"api/projects/{existingProject.Id}");
            Assert.Equal(actualProject.Name, value.Name);
            Assert.Equal(actualProject.Description, value.Description);
        }

        private async Task<ProjectDto> PrepareProject()
        {
            var createProjectDto = new CreateProjectCommand
            {
                Name = "My Fancy project"
            };
            var response = await this.context.Client.PostAsJsonAsync("api/projects", createProjectDto);
            var projectDto = await response.ReadAsAsync<ProjectDto>();
            return projectDto;
        }
    }
}
