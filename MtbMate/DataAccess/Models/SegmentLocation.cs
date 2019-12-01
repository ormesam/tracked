using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class SegmentLocation
    {
        public int SegmentLocationId { get; set; }
        public int SegmentId { get; set; }
        public int Order { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public virtual Segment Segment { get; set; }
    }
}
