using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class UserJumpAchievement {
        [Key]
        public int UserJumpAchievementId { get; set; }
        public int UserId { get; set; }
        public int JumpAchievementId { get; set; }
        public int RideId { get; set; }

        [ForeignKey(nameof(JumpAchievementId))]
        public virtual JumpAchievement JumpAchievement { get; set; }
        [ForeignKey(nameof(RideId))]
        public virtual Ride Ride { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
