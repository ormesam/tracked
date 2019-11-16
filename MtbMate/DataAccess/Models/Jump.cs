using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Jump
    {
        public int JumpId { get; set; }
        public int RideId { get; set; }
        public int Number { get; set; }
        public DateTime Time { get; set; }
        public decimal Airtime { get; set; }
        public decimal LandingGforce { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
