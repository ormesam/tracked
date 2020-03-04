using MtbMate.Models;

namespace MtbMate.Accelerometer {
    public delegate void AccelerometerChangedEventHandler(AccelerometerChangedEventArgs e);

    public class AccelerometerChangedEventArgs {
        public AccelerometerReading Data { get; set; }
    }

    public delegate void AccelerometerStatusChangedEventHandler(AccelerometerStatusChangedEventArgs e);

    public class AccelerometerStatusChangedEventArgs {
        public AccelerometerStatus NewStatus { get; set; }
    }
}
