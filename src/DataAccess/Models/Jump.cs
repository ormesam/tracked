using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class Jump
    {
        public int JumpId { get; set; }
        public int RideId { get; set; }
        public int Number { get; set; }
        public DateTime Timestamp { get; set; }
        public double Airtime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
