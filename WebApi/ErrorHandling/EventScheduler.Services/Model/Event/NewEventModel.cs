using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScheduler.Services.Model.Event
{
    public abstract class BaseEventModel
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        public string? Location { get; set; }
        public bool IsOnline { get; set; }
        public int? MaxCapacity { get; set; }
    }
    public class NewEventModel: BaseEventModel
    {

    }
}
