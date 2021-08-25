using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("UserDistanceAchievement")]
    public partial class UserDistanceAchievement
    {
        [Key]
        public int UserDistanceAchievementId { get; set; }
        public int UserId { get; set; }
        public int DistanceAchievementId { get; set; }
        public int RideId { get; set; }

        [ForeignKey(nameof(DistanceAchievementId))]
        [InverseProperty("UserDistanceAchievements")]
        public virtual DistanceAchievement DistanceAchievement { get; set; }
        [ForeignKey(nameof(RideId))]
        [InverseProperty("UserDistanceAchievements")]
        public virtual Ride Ride { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserDistanceAchievements")]
        public virtual User User { get; set; }
    }
}
