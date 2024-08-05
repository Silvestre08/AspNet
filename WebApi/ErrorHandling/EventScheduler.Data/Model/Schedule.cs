using System;
using System.Collections.Generic;

namespace EventScheduler.Data.Model
{
    public partial class Schedule
    {
        public Guid Id { get; set; }
        public DateTime Time { get; set; }
        public Guid SpeakerId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid EventId { get; set; }

        public virtual Event Event { get; set; } = null!;
        public virtual Speaker Speaker { get; set; } = null!;
    }
}
