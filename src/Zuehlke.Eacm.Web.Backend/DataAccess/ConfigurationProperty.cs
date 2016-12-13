using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zuehlke.Eacm.Web.Backend.DataAccess
{
    public class ConfigurationProperty : IDataModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(200)]
        public string PropertyType { get; set; }

        public Guid EntityId { get; set; }

        [ForeignKey("EntityId")]
        public ConfigurationEntity Entity { get; set; }
    }
}