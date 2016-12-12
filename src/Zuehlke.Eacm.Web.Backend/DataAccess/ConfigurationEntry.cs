using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zuehlke.Eacm.Web.Backend.DataAccess
{
    public class ConfigurationEntry
    {
        [Key]
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        [ForeignKey("EntityId")]
        public ConfigurationEntity Entity { get; set; }

        public ICollection<ConfigurationValue> Values { get; set; }
    }
}