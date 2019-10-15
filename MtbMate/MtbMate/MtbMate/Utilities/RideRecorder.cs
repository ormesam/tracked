using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MtbMate.Accelerometer;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class RideRecorder {
        private readonly bool detectJumps;
        private readonly IList<AccelerometerReading> readings;

        public readonly Ride Ride;

        public RideRecorder(bool detectJumps) {
            this.detectJumps = detectJumps;
            Ride = new Ride();
            readings = new List<AccelerometerReading>();
        }

        public async Task StartRide() {
            if (Ride.Start == null) {
                Ride.Start = DateTime.UtcNow;
            }

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

            await Model.Instance.SaveRide(Ride);

            if (detectJumps) {
                Ride.AccelerometerReadings = readings;
                Ride.Jumps = new JumpDetectionUtility(readings.ToList()).Run();
            }

            await Model.Instance.SaveRide(Ride);

            await CompareSegments();

            await AchievementUtility.AnalyseRide(Ride);
        }

        private async Task CompareSegments() {
            foreach (var segment in Model.Instance.Segments) {
                SegmentAttempt result = Ride.MatchesSegment(segment);

                if (result != null) {
                    await Model.Instance.SaveSegmentAttempt(result);
                }
            }
        }

        private void AccelerometerUtility_AccelerometerChanged(AccelerometerChangedEventArgs e) {
            readings.Add(e.Data);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e) {
            Ride.Locations.Add(e.Location);
        }
    }
}
