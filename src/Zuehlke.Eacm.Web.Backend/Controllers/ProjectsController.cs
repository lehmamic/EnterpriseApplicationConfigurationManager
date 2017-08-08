using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CQRSlite.Commands;
using Microsoft.AspNetCore.Mvc;
using Zuehlke.Eacm.Web.Backend.Commands;
using Zuehlke.Eacm.Web.Backend.DataAccess;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.Models;
using Zuehlke.Eacm.Web.Backend.Utils.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Zuehlke.Eacm.Web.Backend.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly IMapper mapper;
        private readonly EacmDbContext dbContext;
        private readonly ICommandSender commandSender;

        public ProjectsController(EacmDbContext dbContext, ICommandSender commandSender, IMapper mapper)
        {
            this.dbContext = dbContext.ArgumentNotNull(nameof(dbContext));
            this.commandSender = commandSender.ArgumentNotNull(nameof(commandSender));
            this.mapper = mapper.ArgumentNotNull(nameof(mapper));
        }

        [HttpGet("{id}", Name = "GetProject")]
        public async Task<IActionResult> GetProject(Guid id)
        {
			var project = await this.dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.mapper.Map<ProjectDto>(project));
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
			var projects = await this.dbContext.Projects.ToListAsync();

            return this.Ok(this.mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody]ProjectDto project)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var command = this.mapper.Map<CreateProjectCommand>(project);
			await this.commandSender.Send(command);

			var projectReadModel = await this.dbContext.Projects.FirstAsync(p => p.Id == command.Id);
            return this.CreatedAtRoute("GetProject", new { projectReadModel.Id }, this.mapper.Map<ProjectDto>(projectReadModel));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody]ProjectDto project)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

			var currentproject = await this.dbContext.Projects.SingleOrDefaultAsync(p => p.Id == id);
            if (currentproject == null)
            {
                return this.NotFound();
            }

            var command = this.mapper.Map<ModifyProjectCommand>(project, id, currentproject.Version);
			await this.commandSender.Send(command);

            return this.NoContent();
        }

        [HttpGet("{projectId}/entities/{id}", Name = "GetEntity")]
        public async Task<IActionResult> GetEntity(Guid projectId, Guid id)
        {
			var entity = await this.dbContext.Entities.FirstOrDefaultAsync(p => p.Id == id && p.ProjectId == projectId);
            if (entity == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.mapper.Map<EntityDto>(entity));
        }

        [HttpGet("{projectId}/entities")]
        public async Task<IActionResult> GetEntities(Guid projectId)
        {
			var entities = await this.dbContext.Entities.Where(p => p.ProjectId == projectId).ToListAsync();

            return this.Ok(this.mapper.Map<IEnumerable<EntityDto>>(entities));
        }

        [HttpPost("{projectId}/entities")]
        public async Task<IActionResult> CreateEntity(Guid projectId, [FromBody]EntityDto entity)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentProject = await this.dbContext.Projects.SingleOrDefaultAsync(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NotFound();
            }

            var command = this.mapper.Map<CreateEntityCommand>(entity, projectId, currentProject.Version);
			await this.commandSender.Send(command);

            var entityReadModel = await this.dbContext.Entities.FirstAsync(p => p.Name == entity.Name);
            return this.CreatedAtRoute(
                "GetEntity",
                new { ProjectId = projectId, entityReadModel.Id },
                this.mapper.Map<EntityDto>(entityReadModel));
        }

        [HttpPut("{projectId}/entities/{id}")]
        public async Task<IActionResult> UpdateEntity(Guid projectId, Guid id, [FromBody]EntityDto entity)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentProject = await this.dbContext.Projects.SingleOrDefaultAsync(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NotFound();
            }

            entity.Id = id;
            var command = this.mapper.Map<ModifyEntityCommand>(entity, projectId, currentProject.Version);
			await this.commandSender.Send(command);

            return this.NoContent();
        }

        [HttpDelete("{projectId}/entities/{id}")]
        public async Task<IActionResult> DeleteEntity(Guid projectId, Guid id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentProject = await this.dbContext.Projects
                .Include(p => p.Entities)
			    .SingleOrDefaultAsync(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NoContent();
            }
            var currentEntity = await this.dbContext.Entities.FirstOrDefaultAsync(p => p.Id == id);
            if (currentEntity == null)
            {
                return this.NotFound();
            }

            var command = new DeleteEntityCommand
            {
                Id = projectId,
                ExpectedVersion = currentProject.Version,
                EntityId = id
            };

			await this.commandSender.Send(command);

            return this.NoContent();
        }

        [HttpGet("{projectId}/entities/{entityId}/properties/{id}", Name = "GetProperty")]
        public async Task<IActionResult> GetProperty(Guid projectId, Guid entityId, Guid id)
        {
            var currentProject = await this.dbContext.Projects.SingleOrDefaultAsync(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NotFound();
            }

            var currentEntity = await this.dbContext.Entities.FirstOrDefaultAsync(p => p.Id == entityId);
            if (currentEntity == null)
            {
                return this.NotFound();
            }

            var currentProperty = await this.dbContext.Properties.FirstOrDefaultAsync(p => p.Id == id && p.EntityId == entityId);
            if (currentProperty == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.mapper.Map<PropertyDto>(currentProperty));
        }

        [HttpGet("{projectId}/entities/{entityId}/properties")]
        public async Task<IActionResult> GetProperties(Guid projectId, Guid entityId)
        {
            var currentProject = await this.dbContext.Projects
                .Include(p => p.Entities)
                    .ThenInclude(e => e.Properties)
                .SingleOrDefaultAsync(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NotFound();
            }

            var currentEntity = currentProject.Entities
                .FirstOrDefault(p => p.Id == entityId);
            if (currentEntity == null)
            {
                return this.NotFound();
            }

            var properties = currentEntity.Properties
                .Where(p => p.EntityId == entityId);

            return this.Ok(this.mapper.Map<IEnumerable<PropertyDto>>(properties));
        }

        [HttpPost("{projectId}/entities/{entityId}/Properties")]
        public async Task<IActionResult> CreateProperty(Guid projectId, Guid entityId, [FromBody] PropertyDto property)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentProject = await this.dbContext.Projects
                .Include(p => p.Entities)
                .SingleOrDefaultAsync(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NotFound();
            }

            var currentEntity = currentProject.Entities
                .SingleOrDefault(p => p.Id == entityId);
            if (currentEntity == null)
            {
                return this.NotFound();
            }

            var command = this.mapper.Map<CreatePropertyCommand>(property, projectId, currentProject.Version);
            command.ParentEntityId = entityId;

			await this.commandSender.Send(command);

            var propertyReadModel = await this.dbContext.Properties.FirstAsync(p => p.EntityId == entityId && p.Name == property.Name);
            return this.CreatedAtRoute(
                "GetProperty",
                new { ProjectId = projectId, EntityId = entityId, propertyReadModel.Id },
                this.mapper.Map<PropertyDto>(propertyReadModel));
        }

        [HttpPut("{projectId}/entities/{entityId}/properties/{id}", Name = "GetProperty")]
        public async Task<IActionResult> UpdateProperty(Guid projectId, Guid entityId, Guid id, [FromBody]PropertyDto property)
        {
            var currentProject = await this.dbContext.Projects
                .Include(p => p.Entities)
                    .ThenInclude(p => p.Properties)
                .SingleOrDefaultAsync(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NotFound();
            }

            var currentEntity = currentProject.Entities.FirstOrDefault(p => p.Id == entityId && p.ProjectId == projectId);
            if (currentEntity == null)
            {
                return this.NotFound();
            }

            var currentProperty = currentEntity.Properties.FirstOrDefault(p => p.Id == id && p.EntityId == entityId);
            if (currentProperty == null)
            {
                return this.NotFound();
            }

            property.Id = id;
            var command = this.mapper.Map<ModifyPropertyCommand>(property, projectId, currentProject.Version);
			await this.commandSender.Send(command);

            return this.NoContent();
        }

        [HttpDelete("{projectId}/entities/{entityId}/properties/{id}")]
        public async Task<IActionResult> DeleteProperty(Guid projectId, Guid entityId, Guid id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentProject = await this.dbContext.Projects
                .Include(p => p.Entities)
                    .ThenInclude(p => p.Properties)
                .SingleOrDefaultAsync(p => p.Id == projectId);

            if (currentProject == null)
            {
                return this.NoContent();
            }

            var currentEntity = currentProject.Entities.FirstOrDefault(p => p.Id == entityId);
            if (currentEntity == null)
            {
                return this.NoContent();
            }

            var currentProperty = currentEntity.Properties.FirstOrDefault(p => p.Id == id);
            if (currentProperty == null)
            {
                return this.NoContent();
            }

            var command = new DeletePropertyCommand
            {
                Id = projectId,
                ExpectedVersion = currentProject.Version,
                PropertyId = id
            };

			await this.commandSender.Send(command);

            return this.NoContent();
        }
    }
}