using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("RideLocation")]
    public partial class RideLocation
    {
        [Key]
        public int RideLocationId { get; set; }
        public int RideId { get; set; }
        [Column(TypeName = "datetime")]
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
        [InverseProperty("RideLocations")]
        public virtual Ride Ride { get; set; }
    }
}
