using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zuehlke.Eacm.Web.Backend.DataAccess
{
    public class ConfigurationEntity : IDataModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public Guid ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public ConfigurationProject Project { get; set; }

        public ICollection<ConfigurationProperty> Properties { get; set; }

        public ICollection<ConfigurationEntry> Entries { get; set; }
    }
}