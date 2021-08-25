using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("Ride")]
    public partial class Ride
    {
        public Ride()
        {
            AccelerometerReadings = new HashSet<AccelerometerReading>();
            Jumps = new HashSet<Jump>();
            RideLocations = new HashSet<RideLocation>();
            TrailAttempts = new HashSet<TrailAttempt>();
            UserDistanceAchievements = new HashSet<UserDistanceAchievement>();
            UserJumpAchievements = new HashSet<UserJumpAchievement>();
            UserSpeedAchievements = new HashSet<UserSpeedAchievement>();
        }

        [Key]
        public int RideId { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartUtc { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndUtc { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public double DistanceMiles { get; set; }
        public double MaxSpeedMph { get; set; }
        public double AverageSpeedMph { get; set; }
        [Required]
        public string RouteSvgPath { get; set; }
        public int AnalyserVersion { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Rides")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(AccelerometerReading.Ride))]
        public virtual ICollection<AccelerometerReading> AccelerometerReadings { get; set; }
        [InverseProperty(nameof(Jump.Ride))]
        public virtual ICollection<Jump> Jumps { get; set; }
        [InverseProperty(nameof(RideLocation.Ride))]
        public virtual ICollection<RideLocation> RideLocations { get; set; }
        [InverseProperty(nameof(TrailAttempt.Ride))]
        public virtual ICollection<TrailAttempt> TrailAttempts { get; set; }
        [InverseProperty(nameof(UserDistanceAchievement.Ride))]
        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievements { get; set; }
        [InverseProperty(nameof(UserJumpAchievement.Ride))]
        public virtual ICollection<UserJumpAchievement> UserJumpAchievements { get; set; }
        [InverseProperty(nameof(UserSpeedAchievement.Ride))]
        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievements { get; set; }
    }
}
