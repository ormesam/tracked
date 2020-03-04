using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            Ride = new HashSet<Ride>();
            Segment = new HashSet<Segment>();
            SegmentAttempt = new HashSet<SegmentAttempt>();
        }

        public int UserId { get; set; }
        public string GoogleUserId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ride> Ride { get; set; }
        public virtual ICollection<Segment> Segment { get; set; }
        public virtual ICollection<SegmentAttempt> SegmentAttempt { get; set; }
    }
}
