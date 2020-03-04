using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class RideDto {
        public int? RideId { get; set; }
        public Guid? ClientId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IList<RideLocationDto> Locations { get; set; }
        public IList<JumpDto> Jumps { get; set; }
        public IList<AccelerometerReadingDto> AccelerometerReadings { get; set; }
    }
}
