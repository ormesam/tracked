using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class SpeedAchievement
    {
        public SpeedAchievement()
        {
            UserSpeedAchievements = new HashSet<UserSpeedAchievement>();
        }

        public int SpeedAchievementId { get; set; }
        public string Name { get; set; }
        public double MinMph { get; set; }

        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievements { get; set; }
    }
}
