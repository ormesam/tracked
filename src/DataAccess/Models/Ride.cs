using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
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

        public int RideId { get; set; }
        public int UserId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Name { get; set; }
        public double DistanceMiles { get; set; }
        public double MaxSpeedMph { get; set; }
        public double AverageSpeedMph { get; set; }
        public string RouteSvgPath { get; set; }
        public int AnalyserVersion { get; set; }

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
