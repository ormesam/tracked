using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class UserSpeedAchievement
    {
        public int UserSpeedAchievementId { get; set; }
        public int UserId { get; set; }
        public int SpeedAchievementId { get; set; }
        public int RideId { get; set; }

        public virtual Ride Ride { get; set; }
        public virtual SpeedAchievement SpeedAchievement { get; set; }
        public virtual User User { get; set; }
    }
}
