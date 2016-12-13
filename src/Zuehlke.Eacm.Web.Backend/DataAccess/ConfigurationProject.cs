using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zuehlke.Eacm.Web.Backend.DataAccess
{
    public class ConfigurationProject : IDataModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTimeOffset TimeStamp { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public ICollection<ConfigurationEntity> Entities { get; set; }
    }
}