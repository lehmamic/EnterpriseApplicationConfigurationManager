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
        public IActionResult CreateProject([FromBody]CreateProjectCommand command)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (command.Id == Guid.Empty)
            {
                command.Id = Guid.NewGuid();
            }

            this.commandSender.Send(command);

            var project = this.dbContext.Projects.First(p => p.Id == command.Id);
            return this.CreatedAtRoute("GetProject", new {  project.Id }, this.mapper.Map<ProjectDto>(project));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(Guid id, [FromBody] ProjectDto project)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var command = this.mapper.Map<ModifyProjectAttributesCommand>(project, o => o.AfterMap((src, dest) => ((ModifyProjectAttributesCommand) dest).Id = project.Id));
            this.commandSender.Send(command);

            return this.Ok();
        }
    }
}