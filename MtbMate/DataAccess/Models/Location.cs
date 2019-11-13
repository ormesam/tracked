using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Location
    {
        public int LocationId { get; set; }
        public int RideId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal AccuracyInMetres { get; set; }
        public decimal SpeedMetresPerSecond { get; set; }
        public decimal Altitude { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
