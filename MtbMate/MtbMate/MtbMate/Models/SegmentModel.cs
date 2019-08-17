using System;
using System.Collections.Generic;
using System.Linq;

namespace MtbMate.Models
{
    public class SegmentModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IList<LatLongModel> Points { get; set; }
        public LatLongModel Start => Points.FirstOrDefault();
        public LatLongModel End => Points.LastOrDefault();
    }
}
