using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class TrailLocation
    {
        public int TrailLocationId { get; set; }
        public int TrailId { get; set; }
        public int Order { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public virtual Trail Trail { get; set; }
    }
}
