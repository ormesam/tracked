using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("User")]
    [Index(nameof(GoogleUserId), Name = "UQ__User__437CD197EF2F3988", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            Rides = new HashSet<Ride>();
            TrailAttempts = new HashSet<TrailAttempt>();
            Trails = new HashSet<Trail>();
            UserDistanceAchievements = new HashSet<UserDistanceAchievement>();
            UserJumpAchievements = new HashSet<UserJumpAchievement>();
            UserSpeedAchievements = new HashSet<UserSpeedAchievement>();
        }

        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(255)]
        public string GoogleUserId { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public string Bio { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedUtc { get; set; }
        [Required]
        [StringLength(255)]
        public string ProfileImageUrl { get; set; }
        public bool IsAdmin { get; set; }
        [Required]
        [StringLength(255)]
        public string RefreshToken { get; set; }

        [InverseProperty(nameof(Ride.User))]
        public virtual ICollection<Ride> Rides { get; set; }
        [InverseProperty(nameof(TrailAttempt.User))]
        public virtual ICollection<TrailAttempt> TrailAttempts { get; set; }
        [InverseProperty(nameof(Trail.User))]
        public virtual ICollection<Trail> Trails { get; set; }
        [InverseProperty(nameof(UserDistanceAchievement.User))]
        public virtual ICollection<UserDistanceAchievement> UserDistanceAchievements { get; set; }
        [InverseProperty(nameof(UserJumpAchievement.User))]
        public virtual ICollection<UserJumpAchievement> UserJumpAchievements { get; set; }
        [InverseProperty(nameof(UserSpeedAchievement.User))]
        public virtual ICollection<UserSpeedAchievement> UserSpeedAchievements { get; set; }
    }
}
