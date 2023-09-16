using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models {
    public class JumpAchievement {
        public JumpAchievement() {
            UserJumpAchievements = new HashSet<UserJumpAchievement>();
        }

        [Key]
        public int JumpAchievementId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public double MinAirtime { get; set; }

        public virtual ICollection<UserJumpAchievement> UserJumpAchievements { get; set; }
    }
}
