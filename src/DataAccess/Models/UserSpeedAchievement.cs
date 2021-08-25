using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("UserSpeedAchievement")]
    public partial class UserSpeedAchievement
    {
        [Key]
        public int UserSpeedAchievementId { get; set; }
        public int UserId { get; set; }
        public int SpeedAchievementId { get; set; }
        public int RideId { get; set; }

        [ForeignKey(nameof(RideId))]
        [InverseProperty("UserSpeedAchievements")]
        public virtual Ride Ride { get; set; }
        [ForeignKey(nameof(SpeedAchievementId))]
        [InverseProperty("UserSpeedAchievements")]
        public virtual SpeedAchievement SpeedAchievement { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserSpeedAchievements")]
        public virtual User User { get; set; }
    }
}
