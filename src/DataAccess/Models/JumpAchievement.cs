using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("JumpAchievement")]
    public partial class JumpAchievement
    {
        public JumpAchievement()
        {
            UserJumpAchievements = new HashSet<UserJumpAchievement>();
        }

        [Key]
        public int JumpAchievementId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public double MinAirtime { get; set; }

        [InverseProperty(nameof(UserJumpAchievement.JumpAchievement))]
        public virtual ICollection<UserJumpAchievement> UserJumpAchievements { get; set; }
    }
}
