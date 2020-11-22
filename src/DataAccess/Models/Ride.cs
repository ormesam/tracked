using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Ride
    {
        public Ride()
        {
            AccelerometerReading = new HashSet<AccelerometerReading>();
            Jump = new HashSet<Jump>();
            RideLocation = new HashSet<RideLocation>();
            TrailAttempt = new HashSet<TrailAttempt>();
            UserDistanceAchievement = new HashSet<UserDistanceAchievement>();
            UserJumpAchievement = new HashSet<UserJumpAchievement>();
            UserSpeedAchievement = new HashSet<UserSpeedAchievement>();
        }

        public int RideId { get; set; }
        public int UserId { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Name { get; set; }
        public double DistanceMiles { get; set; }
        public double MaxSpeedMph { get; set; }
        public double AverageSpeedMph { get; set; }
        public int RouteCanvasWidthSvg { get; set; }
        public int RouteCanvasHeightSvg { get; set; }
        public string RouteSvgPath { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<AccelerometerReading> AccelerometerReading { get; set; }
        public virtual ICollection<Jump> Jump { get; set; }
        public virtual ICollection<RideLocation> RideLocation { get; set; }
        public virtual ICollection<TrailAttempt> TrailAttempt { get; set; }
        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievement { get; set; }
        public virtual ICollection<UserJumpAchievement> UserJumpAchievement { get; set; }
        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievement { get; set; }
    }
}
