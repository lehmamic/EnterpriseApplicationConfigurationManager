using System;

namespace Zuehlke.Eacm.Web.Backend.Models
{
    public class PropertyDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string PropertyType { get; set; }
    }
}