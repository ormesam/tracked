using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class Ride {
        public Ride() {
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
        public DateTime StartUtc { get; set; }
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
        public virtual User User { get; set; }
        public virtual ICollection<AccelerometerReading> AccelerometerReadings { get; set; }
        public virtual ICollection<Jump> Jumps { get; set; }
        public virtual ICollection<RideLocation> RideLocations { get; set; }
        public virtual ICollection<TrailAttempt> TrailAttempts { get; set; }
        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievements { get; set; }
        public virtual ICollection<UserJumpAchievement> UserJumpAchievements { get; set; }
        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievements { get; set; }
    }
}
