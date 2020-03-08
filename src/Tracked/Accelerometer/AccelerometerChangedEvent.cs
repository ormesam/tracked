using Shared.Dtos;

namespace Tracked.Accelerometer {
    public delegate void AccelerometerChangedEventHandler(AccelerometerChangedEventArgs e);

    public class AccelerometerChangedEventArgs {
        public AccelerometerReadingDto Data { get; set; }
    }

    public delegate void AccelerometerStatusChangedEventHandler(AccelerometerStatusChangedEventArgs e);

    public class AccelerometerStatusChangedEventArgs {
        public AccelerometerStatus NewStatus { get; set; }
    }
}
