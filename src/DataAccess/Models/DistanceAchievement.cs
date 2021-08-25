using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("DistanceAchievement")]
    public partial class DistanceAchievement
    {
        public DistanceAchievement()
        {
            UserDistanceAchievements = new HashSet<UserDistanceAchievement>();
        }

        [Key]
        public int DistanceAchievementId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public double MinDistanceMiles { get; set; }

        [InverseProperty(nameof(UserDistanceAchievement.DistanceAchievement))]
        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievements { get; set; }
    }
}
