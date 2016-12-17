using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            Assert.Null(projectDto.Description);
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
        public async void GetProjects_WithoutQueryOptions_ReturnsProjects()
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
            HttpResponseMessage response =
                await this.context.Client.PutAsJsonAsync($"api/projects/{existingProject.Id}", value);

            // assert
            Assert.True(response.IsSuccessStatusCode);

            var actualProject = await this.context.Client.ReadAsAsync<ProjectDto>($"api/projects/{existingProject.Id}");
            Assert.Equal(actualProject.Name, value.Name);
            Assert.Equal(actualProject.Description, value.Description);
        }

        [Fact]
        public async void CreateEntity_WithValidEntityNameAndDescription_CreatesEntity()
        {
            // arrange
            var existingProject = await this.PrepareProject();

            var value = new EntityDto
            {
                Name = "New Entity Name",
                Description = "New Entity Description"
            };

            // act
            HttpResponseMessage response =
                await this.context.Client.PostAsJsonAsync($"api/projects/{existingProject.Id}/entities", value);

            // arrange
            Assert.True(response.IsSuccessStatusCode);

            var entityDto = await response.ReadAsAsync<EntityDto>();
            Assert.Equal(entityDto.Name, entityDto.Name);
            Assert.Equal(entityDto.Description, entityDto.Description);
        }

        [Fact]
        public async void GetEntity_WithValidProjectAndEntityId_ReturnsEntity()
        {
            // arrange
            var existingProject = await this.PrepareProject();
            var existingEntity = await this.PrepareEntity(existingProject.Id);

            // act
            HttpResponseMessage response = await this.context.Client.GetAsync($"api/projects/{existingProject.Id}/entities/{existingEntity.Id}");

            // arrange
            Assert.True(response.IsSuccessStatusCode);

            var entityDto = await response.ReadAsAsync<EntityDto>();
            Assert.Equal(entityDto.Name, entityDto.Name);
            Assert.Equal(entityDto.Description, entityDto.Description);
        }

        [Fact]
        public async void GetEntities_WithoutQueryOptions_ReturnsEntities()
        {
            // arrange
            var existingProject = await this.PrepareProject();
            var existingEntity1 = await this.PrepareEntity(existingProject.Id);
            var existingEntity2 = await this.PrepareEntity(existingProject.Id);

            // act
            HttpResponseMessage response = await this.context.Client.GetAsync($"api/projects/{existingProject.Id}/entities");

            // arrange
            Assert.True(response.IsSuccessStatusCode);

            var actualEntities = (await response.ReadAsAsync<IEnumerable<EntityDto>>()).ToArray();
            Assert.Contains(actualEntities, p => p.Id == existingEntity1.Id);
            Assert.Contains(actualEntities, p => p.Id == existingEntity2.Id);
        }

        [Fact]
        public async void UpdateEntity_WithValidEntityNameAndDescription_UpdatesEntity()
        {
            // arrange
            var existingProject = await this.PrepareProject();
            var existingEntity = await this.PrepareEntity(existingProject.Id);

            var value = new EntityDto
            {
                Name = "New Entity Name",
                Description = "New Entity Description"
            };

            // act
            HttpResponseMessage response =
                await this.context.Client.PutAsJsonAsync($"api/projects/{existingProject.Id}/entities/{existingEntity.Id}", value);

            // arrange
            Assert.True(response.IsSuccessStatusCode);

            var actualEntity = await this.context.Client.ReadAsAsync<EntityDto>($"api/projects/{existingProject.Id}/entities/{existingEntity.Id}");
            Assert.Equal(actualEntity.Name, value.Name);
            Assert.Equal(actualEntity.Description, value.Description);
        }

        [Fact]
        public async void DeleteEntity_WithValidEntityAndProjectId_RemovesEntity()
        {
            // arrange
            var existingProject = await this.PrepareProject();
            var existingEntity = await this.PrepareEntity(existingProject.Id);

            // act
            HttpResponseMessage deleteResponse =
                await this.context.Client.DeleteAsync($"api/projects/{existingProject.Id}/entities/{existingEntity.Id}");

            // arrange
            Assert.True(deleteResponse.IsSuccessStatusCode);

            var readResponse = await this.context.Client.GetAsync($"api/projects/{existingProject.Id}/entities/{existingEntity.Id}");
            Assert.Equal(HttpStatusCode.NotFound, readResponse.StatusCode);
        }

        // [Fact]
        public async void CreateProperty_WithValidPropertyValues_CreatesProperty()
        {
            // arrange
            var existingProject = await this.PrepareProject();
            var existingEntity = await this.PrepareEntity(existingProject.Id);

            var value = new PropertyDto
            {
                Name = "New Property Name",
                Description = "New Property Description",
                PropertyType = "Zuehlke.Eacm.String"
            };

            // act
            HttpResponseMessage response =
                await this.context.Client.PostAsJsonAsync($"api/projects/{existingProject.Id}/entities/{existingEntity.Id}/properties", value);

            // arrange
            Assert.True(response.IsSuccessStatusCode);

            var propertyDto = await response.ReadAsAsync<PropertyDto>();
            Assert.Equal(propertyDto.Name, propertyDto.Name);
            Assert.Equal(propertyDto.Description, propertyDto.Description);
            Assert.Equal(propertyDto.PropertyType, propertyDto.PropertyType);
        }

        private async Task<ProjectDto> PrepareProject()
        {
            var createProjectDto = new ProjectDto
            {
                Name = "My Fancy project"
            };

            var response = await this.context.Client.PostAsJsonAsync("api/projects", createProjectDto);
            return await response.ReadAsAsync<ProjectDto>();
        }

        private async Task<EntityDto> PrepareEntity(Guid projectId)
        {
            var createEntityDto = new EntityDto
            {
                Name = $"My Fancy Entity {Guid.NewGuid()}",
                Description = "Any Description"
            };

            var response = await this.context.Client.PostAsJsonAsync($"api/projects/{projectId}/entities", createEntityDto);
            return await response.ReadAsAsync<EntityDto>();
        }
    }
}
