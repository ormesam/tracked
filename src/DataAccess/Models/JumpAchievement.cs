using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess.Models
{
    public partial class JumpAchievement
    {
        public JumpAchievement()
        {
            UserJumpAchievements = new HashSet<UserJumpAchievement>();
        }

        public int JumpAchievementId { get; set; }
        public string Name { get; set; }
        public double MinAirtime { get; set; }

        public virtual ICollection<UserJumpAchievement> UserJumpAchievements { get; set; }
    }
}
