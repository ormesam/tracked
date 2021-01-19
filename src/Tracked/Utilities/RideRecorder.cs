using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.JumpDetection;
using Xamarin.Essentials;

namespace Tracked.Utilities {
    /*
        We do not handle the starting and stopping of the location listener in this class,
        we assume the location listening has already been started to "warm up" and have better accuracy.
     */
    public class RideRecorder : IJumpLocationDetector {
        private readonly MainContext context;
        private readonly IList<AccelerometerReadingDto> readings;
        private readonly JumpDetectionUtility jumpDetectionUtility;

        public readonly CreateRideDto Ride;

        public RideRecorder(MainContext context) {
            this.context = context;
            Ride = new CreateRideDto();
            readings = new List<AccelerometerReadingDto>();
            jumpDetectionUtility = new JumpDetectionUtility(this);
        }

        public void StartRide(bool shouldDetectJumps) {
            Ride.StartUtc = DateTime.UtcNow;

            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;

            if (shouldDetectJumps) {
                Accelerometer.Start(SensorSpeed.Game);
            }
        }

        private void Current_PositionChanged(object sender, PositionEventArgs e) {
            var position = e.Position;

            Ride.Locations.Add(new RideLocationDto {
                Latitude = position.Latitude,
                Longitude = position.Longitude,
                AccuracyInMetres = position.Accuracy,
                Altitude = position.Altitude,
                Mph = position.Speed,
                Timestamp = position.Timestamp.UtcDateTime,
            });
        }

        public async Task StopRide() {
            CrossGeolocator.Current.PositionChanged -= Current_PositionChanged;
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;

            if (Accelerometer.IsMonitoring) {
                Accelerometer.Stop();
            }

            Ride.EndUtc = DateTime.UtcNow;
            Ride.AccelerometerReadings = readings;
            Ride.Jumps = jumpDetectionUtility.Jumps;

            if (Ride.Locations.Count > 2) {
                await context.Model.SaveRideUpload(Ride);
            }
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

        public RideLocationDto GetLastLocation(DateTime time) {
            return Ride.Locations
                .Where(i => i.Timestamp <= time)
                .OrderByDescending(i => i.Timestamp)
                .FirstOrDefault();
        }
    }
}
