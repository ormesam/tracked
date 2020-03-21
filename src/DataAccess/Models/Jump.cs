using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Jump
    {
        public Jump()
        {
            SegmentAttemptJump = new HashSet<SegmentAttemptJump>();
        }

        public int JumpId { get; set; }
        public int RideId { get; set; }
        public int Number { get; set; }
        public DateTime Timestamp { get; set; }
        public double Airtime { get; set; }

        public virtual Ride Ride { get; set; }
        public virtual ICollection<SegmentAttemptJump> SegmentAttemptJump { get; set; }
    }
}
