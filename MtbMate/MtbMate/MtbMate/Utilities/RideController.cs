using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MtbMate.Accelerometer;
using MtbMate.Models;

namespace MtbMate.Utilities
{
    public class RideController
    {
        public readonly RideModel Ride;
        private readonly IAccelerometerUtility accelerometerUtility;

        public RideController(IAccelerometerUtility accelerometerUtility)
        {
            Ride = new RideModel();
            this.accelerometerUtility = accelerometerUtility;
        }

        public async Task StartRide()
        {
            if (Ride.Start == null)
            {
                Ride.Start = DateTime.UtcNow;
            }

            accelerometerUtility.AccelerometerChanged += AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged += GeoUtility_LocationChanged;

            GeoUtility.Instance.Start();
            await accelerometerUtility.Start();
        }

        public async Task StopRide()
        {
            Ride.End = DateTime.UtcNow;

            GeoUtility.Instance.Stop();
            await accelerometerUtility.Stop();

            accelerometerUtility.AccelerometerChanged -= AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged -= GeoUtility_LocationChanged;
        }

        private void AccelerometerUtility_AccelerometerChanged(AccelerometerChangedEventArgs e)
        {
            Ride.AccelerometerReadings.Add(e.Data);
            Debug.WriteLine(e.Data);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e)
        {
            Ride.Locations.Add(e.Location);
        }
    }
}
