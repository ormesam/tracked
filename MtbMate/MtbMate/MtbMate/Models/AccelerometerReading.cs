using System;

namespace MtbMate.Models {
    public class AccelerometerReading {
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }

        public override string ToString() {
            return $"{Timestamp} Reading: {Value}";
        }
    }
}
