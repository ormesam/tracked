using System;

namespace Shared.Dtos {
    public class AccelerometerReadingDto {
        public int? RideId { get; set; }
        public DateTime Timestamp { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override string ToString() {
            return $"{Timestamp} X: {X} Y: {Y} Z: {Z}";
        }
    }
}
