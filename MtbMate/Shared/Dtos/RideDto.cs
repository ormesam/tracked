using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class RideDto {
        public int? RideId { get; set; }
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IList<LocationDto> Locations { get; set; }
        public IList<JumpDto> Jumps { get; set; }
    }
}
