using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScheduler.Data.Model
{
    
    public partial class ApplicationUser : IdentityUser
    {
         public ApplicationUser()
        {
            Events = new HashSet<Event>();
        }
        public virtual ICollection<Event> Events { get; set; }

    }
}
