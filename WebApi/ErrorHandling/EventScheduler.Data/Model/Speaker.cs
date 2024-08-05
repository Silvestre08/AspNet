using System;
using System.Collections.Generic;

namespace EventScheduler.Data.Model
{
    public partial class Speaker
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Occupation { get; set; }
        public string? Website { get; set; }
        public string? Linkedin { get; set; }
        public string? Twitter { get; set; }
        public Guid EventId { get; set; }

        public virtual Event Event { get; set; } = null!;
    }
}
