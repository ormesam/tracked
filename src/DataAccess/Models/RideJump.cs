using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class RideJump
    {
        public int RideJumpId { get; set; }
        public int RideId { get; set; }
        public int Number { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Airtime { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
