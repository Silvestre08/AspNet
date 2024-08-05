using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace EventScheduler.Data.Model
{
    public partial class Event
    {
        public Event()
        {
            Participants = new HashSet<Participant>();
            Speakers = new HashSet<Speaker>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Location { get; set; }
        public bool IsOnline { get; set; }
        public int? MaxCapacity { get; set; }
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;
        public virtual ICollection<Participant> Participants { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; }
    }
}
