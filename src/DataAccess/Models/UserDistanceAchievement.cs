using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class UserDistanceAchievement {
        [Key]
        public int UserDistanceAchievementId { get; set; }
        public int UserId { get; set; }
        public int DistanceAchievementId { get; set; }
        public int RideId { get; set; }

        [ForeignKey(nameof(DistanceAchievementId))]
        public virtual DistanceAchievement DistanceAchievement { get; set; }
        [ForeignKey(nameof(RideId))]
        public virtual Ride Ride { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
