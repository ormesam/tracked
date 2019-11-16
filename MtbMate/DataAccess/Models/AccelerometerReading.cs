using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class AccelerometerReading
    {
        public int AccelerometerReadingId { get; set; }
        public int RideId { get; set; }
        public DateTime Time { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }

        public virtual Ride Ride { get; set; }
    }
}
