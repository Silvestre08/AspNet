using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScheduler.Services.Model.Event
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Location { get; set; }
        public bool IsOnline { get; set; }
        public int? MaxCapacity { get; set; }
        public string UserId { get; set; } = null!;
        public int ParticipantCount { get; set; }
        public int SpeakerCount { get; set; }
    }
}
