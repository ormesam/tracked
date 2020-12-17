using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Trail
    {
        public Trail()
        {
            TrailAttempts = new HashSet<TrailAttempt>();
            TrailLocations = new HashSet<TrailLocation>();
        }

        public int TrailId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<TrailAttempt> TrailAttempts { get; set; }
        public virtual ICollection<TrailLocation> TrailLocations { get; set; }
    }
}
