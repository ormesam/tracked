using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class AccelerometerReading {
        [Key]
        public int AccelerometerReadingId { get; set; }
        public int RideId { get; set; }
        public DateTime Time { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        [ForeignKey(nameof(RideId))]
        public virtual Ride Ride { get; set; }
    }
}
