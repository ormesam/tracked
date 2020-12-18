using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Shared.Dtos;
using Tracked.Utilities;

namespace Tracked.JumpDetection {
    public class JumpDetectionUtility {
        private readonly IJumpLocationDetector jumpLocationDetector;
        private readonly Queue<AccelerometerReadingDto> readings;
        private readonly IList<AccelerometerReadingDto> takeoffReadings;
        private readonly IList<AccelerometerReadingDto> freefallReadings;
        private bool started;
        private int jumpCount;
        private const double startTolerance = 2;
        private const double minJumpSeconds = 0.25;
        private const double maxJumpSeconds = 3;
        public static double Tolerance = 0.6;

        public IList<JumpDto> Jumps { get; }

        public JumpDetectionUtility(IJumpLocationDetector jumpLocationDetector) {
            this.jumpLocationDetector = jumpLocationDetector;
            readings = new Queue<AccelerometerReadingDto>();
            takeoffReadings = new List<AccelerometerReadingDto>();
            freefallReadings = new List<AccelerometerReadingDto>();
            started = false;
            jumpCount = 1;

            Jumps = new List<JumpDto>();
        }

        public void AddReading(AccelerometerReadingDto reading) {
            if (!started && (reading.X < -startTolerance || reading.X > startTolerance)) {
                started = true;
            }

            if (!started) {
                return;
            }

            Debug.WriteLine(reading);

            readings.Enqueue(reading);

            if (IsFreefallReading(reading)) {
                freefallReadings.Add(reading);
            } else {
                double jumpTime = freefallReadings.GetTime();

                if (jumpTime >= minJumpSeconds && jumpTime <= maxJumpSeconds) {
                    // The jump meets the minimum time requirment so create the jump
                    freefallReadings.Add(reading);

                    CreateJump();
                }

                takeoffReadings.Clear();
                freefallReadings.Clear();
            }

            // We don't want to store thousands of readings
            if (readings.Count > 30) {
                readings.Dequeue();
            }
        }

        private bool IsFreefallReading(AccelerometerReadingDto reading) {
            return Math.Abs(reading.X) <= Tolerance &&
                Math.Abs(reading.Y) <= Tolerance &&
                Math.Abs(reading.Z) <= Tolerance;
        }

        private void CreateJump() {
            var lastReadings = readings
                .Where(i => i.Timestamp < freefallReadings.First().Timestamp)
                .OrderByDescending(i => i.Timestamp)
                .ToList();

            double? lastReading = null;

            foreach (var r in lastReadings) {
                if (lastReading != null && r.X > lastReading) {
                    break;
                }

                lastReading = r.X;

                takeoffReadings.Add(r);
            }

            var allReadingsForJump = new List<AccelerometerReadingDto>();
            allReadingsForJump.AddRange(takeoffReadings);
            allReadingsForJump.AddRange(freefallReadings);

            allReadingsForJump = allReadingsForJump
                .OrderBy(i => i.Timestamp)
                .ToList();

            var jump = new JumpDto {
                Number = jumpCount++,
                Timestamp = allReadingsForJump.Select(j => j.Timestamp).Min(),
                Airtime = Math.Round(allReadingsForJump.GetTime(), 3),
            };

            var location = jumpLocationDetector.GetLastLocation(jump.Timestamp);

            if (location?.Mph >= 5) {
                jump.Latitude = location.Latitude;
                jump.Longitude = location.Longitude;

                Jumps.Add(jump);
            }
        }
    }
}
