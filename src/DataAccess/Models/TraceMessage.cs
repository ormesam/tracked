using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class TraceMessage {
        [Key]
        public int TraceMessageId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateUtc { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
