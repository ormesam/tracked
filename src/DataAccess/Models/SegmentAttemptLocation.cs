using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class SegmentAttemptLocation
    {
        public int SegmentAttemptLocationId { get; set; }
        public int SegmentAttemptId { get; set; }
        public int RideLocationId { get; set; }

        public virtual RideLocation RideLocation { get; set; }
        public virtual SegmentAttempt SegmentAttempt { get; set; }
    }
}
