using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("UserFollow")]
    public partial class UserFollow
    {
        [Key]
        public int UserFollowId { get; set; }
        public int UserId { get; set; }
        public int FollowUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FollowedUtc { get; set; }

        [ForeignKey(nameof(FollowUserId))]
        [InverseProperty("UserFollowFollowUsers")]
        public virtual User FollowUser { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserFollowUsers")]
        public virtual User User { get; set; }
    }
}
