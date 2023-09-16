using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class Trail {
        public Trail() {
            TrailAttempts = new HashSet<TrailAttempt>();
            TrailLocations = new HashSet<TrailLocation>();
        }

        [Key]
        public int TrailId { get; set; }
        public int UserId { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public virtual ICollection<TrailAttempt> TrailAttempts { get; set; }
        public virtual ICollection<TrailLocation> TrailLocations { get; set; }
    }
}
