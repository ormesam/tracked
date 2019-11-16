using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Ride
    {
        public Ride()
        {
            Jump = new HashSet<Jump>();
            Location = new HashSet<Location>();
        }

        public int RideId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Jump> Jump { get; set; }
        public virtual ICollection<Location> Location { get; set; }
    }
}
