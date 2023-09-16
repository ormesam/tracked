using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models {
    public class TrailLocation {
        [Key]
        public int TrailLocationId { get; set; }
        public int TrailId { get; set; }
        public int Order { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [ForeignKey(nameof(TrailId))]
        public virtual Trail Trail { get; set; }
    }
}
