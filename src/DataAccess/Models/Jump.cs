using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("Jump")]
    public partial class Jump
    {
        [Key]
        public int JumpId { get; set; }
        public int RideId { get; set; }
        public int Number { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Timestamp { get; set; }
        public double Airtime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [ForeignKey(nameof(RideId))]
        [InverseProperty("Jumps")]
        public virtual Ride Ride { get; set; }
    }
}
