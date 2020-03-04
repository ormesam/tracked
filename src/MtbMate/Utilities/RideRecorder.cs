using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MtbMate.Accelerometer;
using MtbMate.JumpDetection;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class RideRecorder {
        private readonly bool detectJumps;
        private readonly IList<AccelerometerReading> readings;
        private readonly JumpDetectionUtility jumpDetectionUtility;

        public readonly Ride Ride;

        public RideRecorder(bool detectJumps) {
            this.detectJumps = detectJumps;
            Ride = new Ride();
            readings = new List<AccelerometerReading>();
            jumpDetectionUtility = new JumpDetectionUtility(GeoUtility.Instance);
        }

        public async Task StartRide() {
            Ride.Start = DateTime.UtcNow;

            AccelerometerUtility.Instance.AccelerometerChanged += AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged += GeoUtility_LocationChanged;

            await GeoUtility.Instance.Start();

            if (detectJumps) {
                await AccelerometerUtility.Instance.Start();
            }
        }

        public async Task StopRide() {
            Ride.End = DateTime.UtcNow;

            await GeoUtility.Instance.Stop();
            await AccelerometerUtility.Instance.Stop();

            AccelerometerUtility.Instance.AccelerometerChanged -= AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged -= GeoUtility_LocationChanged;

            if (detectJumps) {
                Ride.AccelerometerReadings = readings;
                Ride.Jumps = jumpDetectionUtility.Jumps;
            }

            await Model.Instance.SaveRide(Ride);

            await Model.Instance.CompareSegments(Ride);
        }

        private void AccelerometerUtility_AccelerometerChanged(AccelerometerChangedEventArgs e) {
            jumpDetectionUtility.AddReading(e.Data);
            readings.Add(e.Data);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e) {
            Ride.Locations.Add(e.Location);
        }
    }
}
