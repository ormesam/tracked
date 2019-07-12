using System.Threading.Tasks;
using MtbMate.Models;

namespace MtbMate.Accelerometer
{
    public interface IAccelerometerUtility
    {
        event AccelerometerChangedEventHandler AccelerometerChanged;
        void AddReading(AccelerometerReadingModel reading);
        Task Start();
        Task Stop();
    }
}