using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class TrailAttempt {
        [Key]
        public int TrailAttemptId { get; set; }
        public int UserId { get; set; }
        public int TrailId { get; set; }
        public int RideId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public int Medal { get; set; }

        [ForeignKey(nameof(RideId))]
        public virtual Ride Ride { get; set; }
        [ForeignKey(nameof(TrailId))]
        public virtual Trail Trail { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
