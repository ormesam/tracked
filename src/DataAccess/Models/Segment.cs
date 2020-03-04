using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Segment
    {
        public Segment()
        {
            SegmentAttempt = new HashSet<SegmentAttempt>();
            SegmentLocation = new HashSet<SegmentLocation>();
        }

        public int SegmentId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<SegmentAttempt> SegmentAttempt { get; set; }
        public virtual ICollection<SegmentLocation> SegmentLocation { get; set; }
    }
}
