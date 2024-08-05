using System;
using System.Collections.Generic;

namespace EventScheduler.Data.Model
{
    public partial class Participant
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Occupation { get; set; }

        public virtual Event Event { get; set; } = null!;
    }
}
