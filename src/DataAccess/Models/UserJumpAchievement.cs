using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("UserJumpAchievement")]
    public partial class UserJumpAchievement
    {
        [Key]
        public int UserJumpAchievementId { get; set; }
        public int UserId { get; set; }
        public int JumpAchievementId { get; set; }
        public int RideId { get; set; }

        [ForeignKey(nameof(JumpAchievementId))]
        [InverseProperty("UserJumpAchievements")]
        public virtual JumpAchievement JumpAchievement { get; set; }
        [ForeignKey(nameof(RideId))]
        [InverseProperty("UserJumpAchievements")]
        public virtual Ride Ride { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserJumpAchievements")]
        public virtual User User { get; set; }
    }
}
