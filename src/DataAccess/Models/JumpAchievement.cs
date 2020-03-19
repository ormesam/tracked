using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class JumpAchievement
    {
        public JumpAchievement()
        {
            UserJumpAchievement = new HashSet<UserJumpAchievement>();
        }

        public int JumpAchievementId { get; set; }
        public string Name { get; set; }
        public decimal MinAirtime { get; set; }

        public virtual ICollection<UserJumpAchievement> UserJumpAchievement { get; set; }
    }
}
