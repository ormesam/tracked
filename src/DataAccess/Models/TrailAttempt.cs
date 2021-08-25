using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("TrailAttempt")]
    public partial class TrailAttempt
    {
        [Key]
        public int TrailAttemptId { get; set; }
        public int UserId { get; set; }
        public int TrailId { get; set; }
        public int RideId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartUtc { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndUtc { get; set; }
        public int Medal { get; set; }

        [ForeignKey(nameof(RideId))]
        [InverseProperty("TrailAttempts")]
        public virtual Ride Ride { get; set; }
        [ForeignKey(nameof(TrailId))]
        [InverseProperty("TrailAttempts")]
        public virtual Trail Trail { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("TrailAttempts")]
        public virtual User User { get; set; }
    }
}
