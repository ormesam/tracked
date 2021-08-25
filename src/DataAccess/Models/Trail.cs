using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("Trail")]
    public partial class Trail
    {
        public Trail()
        {
            TrailAttempts = new HashSet<TrailAttempt>();
            TrailLocations = new HashSet<TrailLocation>();
        }

        [Key]
        public int TrailId { get; set; }
        public int UserId { get; set; }
        [StringLength(255)]
        public string Name { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Trails")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(TrailAttempt.Trail))]
        public virtual ICollection<TrailAttempt> TrailAttempts { get; set; }
        [InverseProperty(nameof(TrailLocation.Trail))]
        public virtual ICollection<TrailLocation> TrailLocations { get; set; }
    }
}
