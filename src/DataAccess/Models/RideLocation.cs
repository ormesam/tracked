using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class RideLocation {
        [Key]
        public int RideLocationId { get; set; }
        public int RideId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double AccuracyInMetres { get; set; }
        public double Mph { get; set; }
        public double Altitude { get; set; }
        public bool IsRemoved { get; set; }
        [StringLength(255)]
        public string RemovalReason { get; set; }

        [ForeignKey(nameof(RideId))]
        public virtual Ride Ride { get; set; }
    }
}
