using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zuehlke.Eacm.Web.Backend.DataAccess
{
    public class ConfigurationValue
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(4000)]
        public string Value { get; set; }

        public Guid PropertyId { get; set; }

        [ForeignKey("PropertyId")]
        public ConfigurationProperty Property { get; set; }

        public Guid EntryId { get; set; }

        [ForeignKey("EntryId")]
        public ConfigurationEntry Entry { get; set; }
    }
}