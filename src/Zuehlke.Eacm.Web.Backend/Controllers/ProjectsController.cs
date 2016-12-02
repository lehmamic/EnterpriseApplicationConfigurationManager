using System;
using CQRSlite.Commands;
using Microsoft.AspNetCore.Mvc;
using Zuehlke.Eacm.Web.Backend.Commands;
using Zuehlke.Eacm.Web.Backend.Diagnostics;

namespace Zuehlke.Eacm.Web.Backend.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly ICommandSender commandSender;

        public ProjectsController(ICommandSender commandSender)
        {
            this.commandSender = commandSender.ArgumentNotNull(nameof(commandSender));
        }

        [HttpGet("{id}", Name = "GetProject")]
        public IActionResult GetProject(Guid id)
        {
            return this.Ok();
        }

        [HttpPost]
        public IActionResult CreateProject(CreateProjectCommand command)
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

            return this.CreatedAtRoute("GetProject", null);
        }
    }
}