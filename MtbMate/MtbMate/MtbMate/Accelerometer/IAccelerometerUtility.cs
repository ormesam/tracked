using MtbMate.Models;

namespace MtbMate.Accelerometer
{
    public interface IAccelerometerUtility
    {
        event AccelerometerChangedEventHandler AccelerometerChanged;
        void AddReading(AccelerometerReadingModel reading);
        void Start();
        void Stop();
    }
}