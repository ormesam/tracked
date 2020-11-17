using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Trail
    {
        public Trail()
        {
            TrailAttempt = new HashSet<TrailAttempt>();
            TrailLocation = new HashSet<TrailLocation>();
        }

        public int TrailId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<TrailAttempt> TrailAttempt { get; set; }
        public virtual ICollection<TrailLocation> TrailLocation { get; set; }
    }
}
