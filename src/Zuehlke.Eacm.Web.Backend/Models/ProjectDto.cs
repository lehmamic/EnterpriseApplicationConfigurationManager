using System;

namespace Zuehlke.Eacm.Web.Backend.Models
{
    public class ProjectDto
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}