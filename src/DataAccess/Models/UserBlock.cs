using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class UserBlock {
        [Key]
        public int UserBlockId { get; set; }
        public int UserId { get; set; }
        public int BlockUserId { get; set; }
        public DateTime BlockedUtc { get; set; }

        [ForeignKey(nameof(BlockUserId))]
        public virtual User BlockUser { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
