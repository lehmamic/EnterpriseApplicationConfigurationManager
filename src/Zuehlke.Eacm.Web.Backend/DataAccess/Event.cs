﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Zuehlke.Eacm.Web.Backend.DataAccess
{
    public class Event
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
        public string Payload { get; set; }

        public string CorrelationId { get; set; }
    }
}