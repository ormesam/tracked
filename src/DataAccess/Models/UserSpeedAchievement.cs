using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class UserSpeedAchievement {
        [Key]
        public int UserSpeedAchievementId { get; set; }
        public int UserId { get; set; }
        public int SpeedAchievementId { get; set; }
        public int RideId { get; set; }

        [ForeignKey(nameof(RideId))]
        public virtual Ride Ride { get; set; }
        [ForeignKey(nameof(SpeedAchievementId))]
        public virtual SpeedAchievement SpeedAchievement { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
