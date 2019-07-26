using System.Threading.Tasks;
using MtbMate.Models;

namespace MtbMate.Accelerometer
{
    public interface IAccelerometerUtility
    {
        event AccelerometerChangedEventHandler AccelerometerChanged;
        event AccelerometerStatusChangedEventHandler StatusChanged;
        void AddReading(AccelerometerReadingModel reading);
        Task Start();
        Task Stop();
        AccelerometerStatus Status { get; }
    }
}