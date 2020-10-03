using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class DistanceAchievement
    {
        public DistanceAchievement()
        {
            UserDistanceAchievement = new HashSet<UserDistanceAchievement>();
        }

        public int DistanceAchievementId { get; set; }
        public string Name { get; set; }
        public double MinDistanceMiles { get; set; }

        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievement { get; set; }
    }
}
