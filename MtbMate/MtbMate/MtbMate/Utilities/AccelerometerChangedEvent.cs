using MtbMate.Models;

namespace MtbMate.Utilities
{
    public delegate void AccelerometerChangedEventHandler(AccelerometerChangedEventArgs e);

    public class AccelerometerChangedEventArgs
    {
        public AccelerometerReadingModel Data { get; set; }
    }
}
