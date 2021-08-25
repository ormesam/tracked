using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccess.Models
{
    [Table("TrailLocation")]
    public partial class TrailLocation
    {
        [Key]
        public int TrailLocationId { get; set; }
        public int TrailId { get; set; }
        public int Order { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [ForeignKey(nameof(TrailId))]
        [InverseProperty("TrailLocations")]
        public virtual Trail Trail { get; set; }
    }
}
