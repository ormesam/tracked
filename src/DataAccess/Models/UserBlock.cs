using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("UserBlock")]
    public partial class UserBlock
    {
        [Key]
        public int UserBlockId { get; set; }
        public int UserId { get; set; }
        public int BlockUserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime BlockedUtc { get; set; }

        [ForeignKey(nameof(BlockUserId))]
        [InverseProperty("UserBlockBlockUsers")]
        public virtual User BlockUser { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserBlockUsers")]
        public virtual User User { get; set; }
    }
}
