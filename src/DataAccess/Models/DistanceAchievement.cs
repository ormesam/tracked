using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class DistanceAchievement
    {
        public DistanceAchievement()
        {
            UserDistanceAchievements = new HashSet<UserDistanceAchievement>();
        }

        public int DistanceAchievementId { get; set; }
        public string Name { get; set; }
        public double MinDistanceMiles { get; set; }

        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievements { get; set; }
    }
}
