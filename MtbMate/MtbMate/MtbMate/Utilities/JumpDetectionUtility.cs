using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class JumpDetectionUtility {
        private readonly Queue<AccelerometerReading> readings;
        private readonly IList<AccelerometerReading> takeoffReadings;
        private readonly IList<AccelerometerReading> freefallReadings;
        private bool started;
        private int jumpCount;
        private const double startTolerance = 2;
        private const double minJumpSeconds = 0.3;
        private const double maxJumpSeconds = 3;

        public static double Tolerance = 0.75;

        public IList<Jump> Jumps { get; }

        public JumpDetectionUtility() {
            readings = new Queue<AccelerometerReading>();
            takeoffReadings = new List<AccelerometerReading>();
            freefallReadings = new List<AccelerometerReading>();
            started = false;
            jumpCount = 1;

            Jumps = new List<Jump>();
        }

        public void AddReading(AccelerometerReading reading) {
            if (!started && (reading.X < -startTolerance || reading.X > startTolerance)) {
                started = true;
            }

            if (!started) {
                return;
            }

            Debug.WriteLine(reading);

            readings.Enqueue(reading);

            if (reading.IsFreefallReading()) {
                freefallReadings.Add(reading);
            } else {
                double jumpTime = freefallReadings.GetTime();

                if (jumpTime >= minJumpSeconds && jumpTime <= maxJumpSeconds) {
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

                    var allReadingsForJump = new List<AccelerometerReading>();
                    allReadingsForJump.AddRange(takeoffReadings);
                    allReadingsForJump.AddRange(freefallReadings);
                    allReadingsForJump.Add(reading);

                    allReadingsForJump = allReadingsForJump
                        .OrderBy(i => i.Timestamp)
                        .ToList();

                    var jump = new Jump {
                        Number = jumpCount++,
                        Time = allReadingsForJump.Select(j => j.Timestamp).Min(),
                        Airtime = Math.Round(allReadingsForJump.GetTime(), 3),
                        Readings = allReadingsForJump,
                    };

                    Jumps.Add(jump);
                }

                takeoffReadings.Clear();
                freefallReadings.Clear();
            }

            // We don't want to store thousands of readings
            if (readings.Count > 30) {
                readings.Dequeue();
            }
        }
    }
}
