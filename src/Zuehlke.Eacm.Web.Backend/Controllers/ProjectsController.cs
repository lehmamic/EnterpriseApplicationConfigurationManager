using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CQRSlite.Commands;
using CQRSlite.Domain.Exception;
using Microsoft.AspNetCore.Mvc;
using Zuehlke.Eacm.Web.Backend.Commands;
using Zuehlke.Eacm.Web.Backend.DataAccess;
using Zuehlke.Eacm.Web.Backend.Diagnostics;
using Zuehlke.Eacm.Web.Backend.Models;
using Zuehlke.Eacm.Web.Backend.Utils.Mapper;

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

            var currentproject = this.dbContext.Projects.SingleOrDefault(p => p.Id == projectId);
            if (currentproject == null)
            {
                return this.NotFound();
            }

            var command = this.mapper.Map<CreateEntityCommand>(entity, projectId, currentproject.Version);
            this.commandSender.Send(command);

            var entityReadModel = this.dbContext.Entities.First(p => p.Name == entity.Name);
            return this.CreatedAtRoute(
                "GetEntity",
                new { ProjectId = projectId, entityReadModel.Id },
                this.mapper.Map<EntityDto>(entityReadModel));
        }
    }
}