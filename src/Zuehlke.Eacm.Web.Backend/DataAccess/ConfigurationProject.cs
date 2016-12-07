using System;
using System.ComponentModel.DataAnnotations;

namespace Zuehlke.Eacm.Web.Backend.DataAccess
{
    public class ConfigurationProject
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTimeOffset TimeStamp { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}