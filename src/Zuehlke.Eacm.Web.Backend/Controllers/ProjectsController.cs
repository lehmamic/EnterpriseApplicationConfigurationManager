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
        public IActionResult GetProject(Guid id)
        {
            var project = this.dbContext.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.mapper.Map<ProjectDto>(project));
        }

        [HttpGet]
        public IActionResult GetProjects()
        {
            var projects = this.dbContext.Projects.AsEnumerable();

            return this.Ok(this.mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        [HttpPost]
        public IActionResult CreateProject([FromBody]ProjectDto project)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var command = this.mapper.Map<CreateProjectCommand>(project);
            this.commandSender.Send(command);

            var projectReadModel = this.dbContext.Projects.First(p => p.Id == command.Id);
            return this.CreatedAtRoute("GetProject", new { projectReadModel.Id }, this.mapper.Map<ProjectDto>(projectReadModel));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(Guid id, [FromBody]ProjectDto project)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentproject = this.dbContext.Projects.SingleOrDefault(p => p.Id == id);
            if (currentproject == null)
            {
                return this.NotFound();
            }

            var command = this.mapper.Map<ModifyProjectCommand>(project, id, currentproject.Version);
            this.commandSender.Send(command);

            return this.NoContent();
        }

        [HttpGet("{projectId}/entities/{id}", Name = "GetEntity")]
        public IActionResult GetEntity(Guid projectId, Guid id)
        {
            var entity = this.dbContext.Entities.FirstOrDefault(p => p.Id == id && p.ProjectId == projectId);
            if (entity == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.mapper.Map<EntityDto>(entity));
        }

        [HttpGet("{projectId}/entities")]
        public IActionResult GetEntities(Guid projectId)
        {
            var entities = this.dbContext.Entities.Where(p => p.ProjectId == projectId);

            return this.Ok(this.mapper.Map<IEnumerable<EntityDto>>(entities));
        }

        [HttpPost("{projectId}/entities")]
        public IActionResult CreateEntity(Guid projectId, [FromBody]EntityDto entity)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentProject = this.dbContext.Projects.SingleOrDefault(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NotFound();
            }

            var command = this.mapper.Map<CreateEntityCommand>(entity, projectId, currentProject.Version);
            this.commandSender.Send(command);

            var entityReadModel = this.dbContext.Entities.First(p => p.Name == entity.Name);
            return this.CreatedAtRoute(
                "GetEntity",
                new { ProjectId = projectId, entityReadModel.Id },
                this.mapper.Map<EntityDto>(entityReadModel));
        }

        [HttpPut("{projectId}/entities/{id}")]
        public IActionResult UpdateEntity(Guid projectId, Guid id, [FromBody]EntityDto entity)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentProject = this.dbContext.Projects.SingleOrDefault(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NotFound();
            }

            entity.Id = id;
            var command = this.mapper.Map<ModifyEntityCommand>(entity, projectId, currentProject.Version);
            this.commandSender.Send(command);

            return this.NoContent();
        }

        [HttpDelete("{projectId}/entities/{id}")]
        public IActionResult DeleteEntity(Guid projectId, Guid id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentProject = this.dbContext.Projects
                .Include(p => p.Entities)
                .SingleOrDefault(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NoContent();
            }
            var currentEntity = this.dbContext.Entities.FirstOrDefault(p => p.Id == id);
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

            this.commandSender.Send(command);

            return this.NoContent();
        }

        [HttpGet("{projectId}/entities/{entityId}/properties/{id}", Name = "GetProperty")]
        public IActionResult GetProperty(Guid projectId, Guid entityId, Guid id)
        {
            var currentProject = this.dbContext.Projects.SingleOrDefault(p => p.Id == projectId);
            if (currentProject == null)
            {
                return this.NotFound();
            }

            var currentEntity = this.dbContext.Entities.FirstOrDefault(p => p.Id == entityId);
            if (currentEntity == null)
            {
                return this.NotFound();
            }

            var currentProperty = this.dbContext.Properties.FirstOrDefault(p => p.Id == id && p.EntityId == entityId);
            if (currentProperty == null)
            {
                return this.NotFound();
            }

            return this.Ok(this.mapper.Map<PropertyDto>(currentProperty));
        }

        [HttpGet("{projectId}/entities/{entityId}/properties")]
        public IActionResult GetProperties(Guid projectId, Guid entityId)
        {
            var currentProject = this.dbContext.Projects
                .Include(p => p.Entities)
                    .ThenInclude(e => e.Properties)
                .SingleOrDefault(p => p.Id == projectId);
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
        public IActionResult CreateProperty(Guid projectId, Guid entityId, [FromBody] PropertyDto property)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentProject = this.dbContext.Projects
                .Include(p => p.Entities)
                .SingleOrDefault(p => p.Id == projectId);
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

            this.commandSender.Send(command);

            var propertyReadModel = this.dbContext.Properties.First(p => p.EntityId == entityId && p.Name == property.Name);
            return this.CreatedAtRoute(
                "GetProperty",
                new { ProjectId = projectId, EntityId = entityId, propertyReadModel.Id },
                this.mapper.Map<PropertyDto>(propertyReadModel));
        }

        [HttpPut("{projectId}/entities/{entityId}/properties/{id}", Name = "GetProperty")]
        public IActionResult UpdateProperty(Guid projectId, Guid entityId, Guid id, [FromBody]PropertyDto property)
        {
            var currentProject = this.dbContext.Projects
                .Include(p => p.Entities)
                    .ThenInclude(p => p.Properties)
                .SingleOrDefault(p => p.Id == projectId);
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
            var command = this.mapper.Map<ModifyPropertyCommand>(currentProperty, projectId, currentProject.Version);
            this.commandSender.Send(command);

            return this.NoContent();
        }

        [HttpDelete("{projectId}/entities/{entityId}/properties/{id}")]
        public IActionResult DeleteProperty(Guid projectId, Guid entityId, Guid id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentProject = this.dbContext.Projects
                .Include(p => p.Entities)
                    .ThenInclude(p => p.Properties)
                .SingleOrDefault(p => p.Id == projectId);

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

            var command = new DeleteEntityCommand
            {
                Id = projectId,
                ExpectedVersion = currentProject.Version,
                EntityId = id
            };

            this.commandSender.Send(command);

            return this.NoContent();
        }
    }
}