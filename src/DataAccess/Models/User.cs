using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            Rides = new HashSet<Ride>();
            TrailAttempts = new HashSet<TrailAttempt>();
            Trails = new HashSet<Trail>();
            UserDistanceAchievements = new HashSet<UserDistanceAchievement>();
            UserJumpAchievements = new HashSet<UserJumpAchievement>();
            UserSpeedAchievements = new HashSet<UserSpeedAchievement>();
        }

        public int UserId { get; set; }
        public string GoogleUserId { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedUtc { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool IsAdmin { get; set; }
        public string RefreshToken { get; set; }

        public virtual ICollection<Ride> Rides { get; set; }
        public virtual ICollection<TrailAttempt> TrailAttempts { get; set; }
        public virtual ICollection<Trail> Trails { get; set; }
        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievements { get; set; }
        public virtual ICollection<UserJumpAchievement> UserJumpAchievements { get; set; }
        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievements { get; set; }
    }
}
