using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MtbMate.Accelerometer;
using MtbMate.Models;

namespace MtbMate.Utilities
{
    public class RideController
    {
        private readonly bool detectJumps;
        private readonly AccelerometerUtility accelerometerUtility;

        public readonly RideModel Ride;

        public RideController(bool detectJumps) {
            this.detectJumps = detectJumps;
            Ride = new RideModel();
            accelerometerUtility = AccelerometerUtility.Instance;
        }

        public async Task StartRide() {
            if (Ride.Start == null) {
                Ride.Start = DateTime.UtcNow;
            }

            accelerometerUtility.AccelerometerChanged += AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged += GeoUtility_LocationChanged;

            GeoUtility.Instance.Start();

            if (detectJumps) {
                await accelerometerUtility.Start();
            }
        }

        public async Task StopRide() {
            Ride.End = DateTime.UtcNow;

            GeoUtility.Instance.Stop();
            await accelerometerUtility.Stop();

            accelerometerUtility.AccelerometerChanged -= AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged -= GeoUtility_LocationChanged;
        }

        private void AccelerometerUtility_AccelerometerChanged(AccelerometerChangedEventArgs e) {
            Ride.AccelerometerReadings.Add(e.Data);
            Debug.WriteLine(e.Data);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e) {
            Ride.Locations.Add(e.Location);
        }
    }
}
