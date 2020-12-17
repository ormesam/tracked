using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class AccelerometerReading
    {
        public int AccelerometerReadingId { get; set; }
        public int RideId { get; set; }
        public DateTime Time { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
