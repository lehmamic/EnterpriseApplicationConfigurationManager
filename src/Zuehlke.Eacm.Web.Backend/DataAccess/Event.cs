using System;
using System.ComponentModel.DataAnnotations;

namespace Zuehlke.Eacm.Web.Backend.DataAccess
{
    public class Event : IDataModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid AggregateId { get; set; }

        [Required]
        public string AggregateType { get; set; }

        [Required]
        public DateTimeOffset Timestamp { get; set; }

        [Required]
        public int Version { get; set; }

        [Required]
        [MaxLength(200)]
        public string User { get; set; }

        [Required]
        public string Payload { get; set; }

        public string CorrelationId { get; set; }
    }
}