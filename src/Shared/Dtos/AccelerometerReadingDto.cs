using System;

namespace Shared.Dtos {
    public class AccelerometerReadingDto {
        public int? RideId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public decimal Z { get; set; }

        public override string ToString() {
            return $"{Timestamp} X: {X} Y: {Y} Z: {Z}";
        }
    }
}
