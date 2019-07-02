using MtbMate.Models;

namespace MtbMate.Accelerometer
{
    public delegate void AccelerometerChangedEventHandler(AccelerometerChangedEventArgs e);

    public class AccelerometerChangedEventArgs
    {
        public AccelerometerReadingModel Data { get; set; }
    }
}
