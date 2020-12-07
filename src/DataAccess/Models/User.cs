using System;
using System.Collections.Generic;

namespace DataAccess.Models {
    public partial class User {
        public User() {
            Ride = new HashSet<Ride>();
            Trail = new HashSet<Trail>();
            TrailAttempt = new HashSet<TrailAttempt>();
            UserDistanceAchievement = new HashSet<UserDistanceAchievement>();
            UserJumpAchievement = new HashSet<UserJumpAchievement>();
            UserSpeedAchievement = new HashSet<UserSpeedAchievement>();
        }

        public int UserId { get; set; }
        public string GoogleUserId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool IsAdmin { get; set; }

        public virtual ICollection<Ride> Ride { get; set; }
        public virtual ICollection<Trail> Trail { get; set; }
        public virtual ICollection<TrailAttempt> TrailAttempt { get; set; }
        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievement { get; set; }
        public virtual ICollection<UserJumpAchievement> UserJumpAchievement { get; set; }
        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievement { get; set; }
    }
}
