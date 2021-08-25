using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("AccelerometerReading")]
    public partial class AccelerometerReading
    {
        [Key]
        public int AccelerometerReadingId { get; set; }
        public int RideId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Time { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        [ForeignKey(nameof(RideId))]
        [InverseProperty("AccelerometerReadings")]
        public virtual Ride Ride { get; set; }
    }
}
