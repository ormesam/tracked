using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("SpeedAchievement")]
    public partial class SpeedAchievement
    {
        public SpeedAchievement()
        {
            UserSpeedAchievements = new HashSet<UserSpeedAchievement>();
        }

        [Key]
        public int SpeedAchievementId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public double MinMph { get; set; }

        [InverseProperty(nameof(UserSpeedAchievement.SpeedAchievement))]
        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievements { get; set; }
    }
}
