using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.JumpDetection;
using Xamarin.Essentials;

namespace Tracked.Utilities {
    public class RideRecorder {
        private readonly MainContext context;
        private readonly bool detectJumps;
        private readonly IList<AccelerometerReadingDto> readings;
        private readonly JumpDetectionUtility jumpDetectionUtility;

        public readonly CreateRideDto Ride;

        public RideRecorder(MainContext context) {
            this.context = context;
            this.detectJumps = context.Settings.ShouldDetectJumps;
            Ride = new CreateRideDto();
            readings = new List<AccelerometerReadingDto>();
            jumpDetectionUtility = new JumpDetectionUtility(context.GeoUtility);
        }

        public void StartRide() {
            Ride.StartUtc = DateTime.UtcNow;

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            context.GeoUtility.LocationChanged += GeoUtility_LocationChanged;

            context.GeoUtility.Start();

            if (detectJumps) {
                Accelerometer.Start(SensorSpeed.Fastest);
            }
        }

        public async Task StopRide() {
            if (Accelerometer.IsMonitoring) {
                Accelerometer.Stop();
            }

            context.GeoUtility.LocationChanged -= GeoUtility_LocationChanged;
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;

            Ride.EndUtc = DateTime.UtcNow;

            context.GeoUtility.Stop();

            if (detectJumps) {
                Ride.AccelerometerReadings = readings;
                Ride.Jumps = jumpDetectionUtility.Jumps;
            }

            await context.Model.SaveRideUpload(Ride);
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e) {
            var reading = new AccelerometerReadingDto {
                Timestamp = DateTime.UtcNow,
                X = e.Reading.Acceleration.X,
                Y = e.Reading.Acceleration.Y,
                Z = e.Reading.Acceleration.Z,
            };

            jumpDetectionUtility.AddReading(reading);
            readings.Add(reading);

            Debug.WriteLine(reading);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e) {
            Ride.Locations.Add(e.Location);
        }
    }
}
