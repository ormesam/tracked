using System;

namespace Shared.Dtos {
    public class AccelerometerReadingDto {
        public int? RideId { get; set; }
        public DateTime Time { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }
    }
}
