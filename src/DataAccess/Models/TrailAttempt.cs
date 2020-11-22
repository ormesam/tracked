using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class TrailAttempt
    {
        public int TrailAttemptId { get; set; }
        public int UserId { get; set; }
        public int TrailId { get; set; }
        public int RideId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int Medal { get; set; }

        public virtual Ride Ride { get; set; }
        public virtual Trail Trail { get; set; }
        public virtual User User { get; set; }
    }
}
