using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class UserDistanceAchievement
    {
        public int UserDistanceAchievementId { get; set; }
        public int UserId { get; set; }
        public int DistanceAchievementId { get; set; }
        public int RideId { get; set; }

        public virtual DistanceAchievement DistanceAchievement { get; set; }
        public virtual Ride Ride { get; set; }
        public virtual User User { get; set; }
    }
}
