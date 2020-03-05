using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class SegmentAttempt
    {
        public SegmentAttempt()
        {
            SegmentAttemptJump = new HashSet<SegmentAttemptJump>();
            SegmentAttemptLocation = new HashSet<SegmentAttemptLocation>();
        }

        public int SegmentAttemptId { get; set; }
        public int UserId { get; set; }
        public int SegmentId { get; set; }
        public int RideId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int Medal { get; set; }
        public decimal MaxSpeedMph { get; set; }
        public decimal AverageSpeedMph { get; set; }
        public decimal DistanceMiles { get; set; }

        public virtual Ride Ride { get; set; }
        public virtual Segment Segment { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<SegmentAttemptJump> SegmentAttemptJump { get; set; }
        public virtual ICollection<SegmentAttemptLocation> SegmentAttemptLocation { get; set; }
    }
}
