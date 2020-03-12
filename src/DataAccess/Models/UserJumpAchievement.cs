using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class UserJumpAchievement
    {
        public int UserJumpAchievementId { get; set; }
        public int UserId { get; set; }
        public int JumpAchievementId { get; set; }
        public int RideId { get; set; }

        public virtual JumpAchievement JumpAchievement { get; set; }
        public virtual Ride Ride { get; set; }
        public virtual User User { get; set; }
    }
}
