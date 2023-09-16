using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class UserFollow {
        [Key]
        public int UserFollowId { get; set; }
        public int UserId { get; set; }
        public int FollowUserId { get; set; }
        public DateTime FollowedUtc { get; set; }

        [ForeignKey(nameof(FollowUserId))]
        public virtual User FollowUser { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
