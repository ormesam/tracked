using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models {
    public class SpeedAchievement {
        public SpeedAchievement() {
            UserSpeedAchievements = new HashSet<UserSpeedAchievement>();
        }

        [Key]
        public int SpeedAchievementId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public double MinMph { get; set; }

        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievements { get; set; }
    }
}
