using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Ride
    {
        public Ride()
        {
            AccelerometerReading = new HashSet<AccelerometerReading>();
            Jump = new HashSet<Jump>();
            RideLocation = new HashSet<RideLocation>();
            SegmentAttempt = new HashSet<SegmentAttempt>();
        }

        public int RideId { get; set; }
        public int UserId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<AccelerometerReading> AccelerometerReading { get; set; }
        public virtual ICollection<Jump> Jump { get; set; }
        public virtual ICollection<RideLocation> RideLocation { get; set; }
        public virtual ICollection<SegmentAttempt> SegmentAttempt { get; set; }
    }
}
