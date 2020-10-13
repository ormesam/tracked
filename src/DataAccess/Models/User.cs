using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            Ride = new HashSet<Ride>();
            Segment = new HashSet<Segment>();
            SegmentAttempt = new HashSet<SegmentAttempt>();
            UserDistanceAchievement = new HashSet<UserDistanceAchievement>();
            UserJumpAchievement = new HashSet<UserJumpAchievement>();
            UserSpeedAchievement = new HashSet<UserSpeedAchievement>();
        }

        public int UserId { get; set; }
        public string GoogleUserId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string ProfileImageUrl { get; set; }

        public virtual ICollection<Ride> Ride { get; set; }
        public virtual ICollection<Segment> Segment { get; set; }
        public virtual ICollection<SegmentAttempt> SegmentAttempt { get; set; }
        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievement { get; set; }
        public virtual ICollection<UserJumpAchievement> UserJumpAchievement { get; set; }
        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievement { get; set; }
    }
}
