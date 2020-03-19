using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class SpeedAchievement
    {
        public SpeedAchievement()
        {
            UserSpeedAchievement = new HashSet<UserSpeedAchievement>();
        }

        public int SpeedAchievementId { get; set; }
        public string Name { get; set; }
        public decimal MinMph { get; set; }

        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievement { get; set; }
    }
}
