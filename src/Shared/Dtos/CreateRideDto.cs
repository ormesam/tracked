using System;
using System.Collections.Generic;

namespace Shared.Dtos {
    public class CreateRideDto : RideDto {
        public Guid? Id { get; set; }
        public IList<AccelerometerReadingDto> AccelerometerReadings { get; set; }

        public CreateRideDto() {
            AccelerometerReadings = new List<AccelerometerReadingDto>();
        }
    }
}
