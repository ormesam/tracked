using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class RideUploadDto {
        public Guid? Id { get; set; }
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public IList<RideLocationDto> Locations { get; set; }
        public IList<RideJumpDto> Jumps { get; set; }
        public IList<AccelerometerReadingDto> AccelerometerReadings { get; set; }

        public RideUploadDto() {
            Locations = new List<RideLocationDto>();
            Jumps = new List<RideJumpDto>();
            AccelerometerReadings = new List<AccelerometerReadingDto>();
        }
    }
}
