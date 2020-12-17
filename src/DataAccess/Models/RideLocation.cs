using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class RideLocation
    {
        public int RideLocationId { get; set; }
        public int RideId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double AccuracyInMetres { get; set; }
        public double Mph { get; set; }
        public double Altitude { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
