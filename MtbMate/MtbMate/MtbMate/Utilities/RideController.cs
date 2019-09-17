using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MtbMate.Accelerometer;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class RideController {
        private readonly bool detectJumps;

        public readonly Ride Ride;

        public RideController(bool detectJumps) {
            this.detectJumps = detectJumps;
            Ride = new Ride();
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

            await Model.Instance.SaveRide(Ride);

            new JumpDetectionUtility(Ride).Run();

            await Model.Instance.SaveRide(Ride);

            await CompareSegments();
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
            Ride.AccelerometerReadings.Add(e.Data);
            Debug.WriteLine(e.Data);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e) {
            Ride.Locations.Add(e.Location);
        }
    }
}
