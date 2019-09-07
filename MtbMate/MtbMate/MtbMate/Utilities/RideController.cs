using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using MtbMate.Accelerometer;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class RideController {
        private readonly bool detectJumps;

        public readonly RideModel Ride;
        public IList<SegmentAttemptModel> SegmentAttempts;

        public RideController(bool detectJumps) {
            this.detectJumps = detectJumps;
            Ride = new RideModel();
            SegmentAttempts = new List<SegmentAttemptModel>();
        }

        public async Task StartRide() {
            if (Ride.Start == null) {
                Ride.Start = DateTime.UtcNow;
            }

            AccelerometerUtility.Instance.AccelerometerChanged += AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged += GeoUtility_LocationChanged;

            GeoUtility.Instance.Start();

            if (detectJumps) {
                await AccelerometerUtility.Instance.Start();
            }
        }

        public async Task StopRide() {
            Ride.End = DateTime.UtcNow;

            GeoUtility.Instance.Stop();
            await AccelerometerUtility.Instance.Stop();

            AccelerometerUtility.Instance.AccelerometerChanged -= AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged -= GeoUtility_LocationChanged;

            CompareSegments();
        }

        private void CompareSegments() {
            foreach (var segment in Model.Instance.Segments) {
                SegmentAttemptModel result = Ride.MatchesSegment(segment);

                if (result != null) {
                    SegmentAttempts.Add(result);
                }
            }
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
