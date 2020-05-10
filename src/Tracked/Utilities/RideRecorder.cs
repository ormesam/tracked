using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Accelerometer;
using Tracked.Contexts;
using Tracked.JumpDetection;

namespace Tracked.Utilities {
    public class RideRecorder {
        private readonly MainContext context;
        private readonly bool detectJumps;
        private readonly IList<AccelerometerReadingDto> readings;
        private readonly JumpDetectionUtility jumpDetectionUtility;

        public readonly CreateRideDto Ride;

        public RideRecorder(MainContext context) {
            this.context = context;
            this.detectJumps = context.Settings.DetectJumps;
            Ride = new CreateRideDto();
            readings = new List<AccelerometerReadingDto>();
            jumpDetectionUtility = new JumpDetectionUtility(GeoUtility.Instance);
        }

        public async Task StartRide() {
            Ride.StartUtc = DateTime.UtcNow;

            AccelerometerUtility.Instance.AccelerometerChanged += AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged += GeoUtility_LocationChanged;

            GeoUtility.Instance.Start();

            if (detectJumps) {
                await AccelerometerUtility.Instance.Start();
            }
        }

        public async Task StopRide() {
            AccelerometerUtility.Instance.AccelerometerChanged -= AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged -= GeoUtility_LocationChanged;

            Ride.EndUtc = DateTime.UtcNow;

            await AccelerometerUtility.Instance.Stop();
            GeoUtility.Instance.Stop();

            if (detectJumps) {
                Ride.AccelerometerReadings = readings;
                Ride.Jumps = jumpDetectionUtility.Jumps;
            }

            await context.Model.SaveRideUpload(Ride);
        }

        private void AccelerometerUtility_AccelerometerChanged(AccelerometerChangedEventArgs e) {
            jumpDetectionUtility.AddReading(e.Data);
            readings.Add(e.Data);
            Debug.WriteLine(e.Data);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e) {
            Ride.Locations.Add(e.Location);
        }
    }
}
