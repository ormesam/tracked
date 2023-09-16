using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models {
    public class DistanceAchievement {
        public DistanceAchievement() {
            UserDistanceAchievements = new HashSet<UserDistanceAchievement>();
        }

        [Key]
        public int DistanceAchievementId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public double MinDistanceMiles { get; set; }

        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievements { get; set; }
    }
}
