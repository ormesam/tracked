using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class Jump {
        [Key]
        public int JumpId { get; set; }
        public int RideId { get; set; }
        public int Number { get; set; }
        public DateTime Timestamp { get; set; }
        public double Airtime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [ForeignKey(nameof(RideId))]
        public virtual Ride Ride { get; set; }
    }
}
