using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("TraceMessage")]
    public partial class TraceMessage
    {
        [Key]
        public int TraceMessageId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateUtc { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
