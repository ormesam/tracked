using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class SegmentAttemptLocation
    {
        public int SegmentAttemptLocationId { get; set; }
        public int SegmentAttemptId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal AccuracyInMetres { get; set; }
        public decimal SpeedMetresPerSecond { get; set; }
        public decimal Altitude { get; set; }

        public virtual SegmentAttempt SegmentAttempt { get; set; }
    }
}
