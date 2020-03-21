using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class SegmentLocation
    {
        public int SegmentLocationId { get; set; }
        public int SegmentId { get; set; }
        public int Order { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public virtual Segment Segment { get; set; }
    }
}
