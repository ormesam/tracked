using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class JumpDetectionUtility {
        private readonly Queue<AccelerometerReading> readings;
        private readonly IList<AccelerometerReading> potentialJumpReadings;
        private AccelerometerReading lastReading;
        private Jump lastJump;
        private bool started;
        private int readingsSinceLastJump;
        private int jumpCount;
        private const double startTolerance = 2;
        private const double minJumpSeconds = 0.5;
        private const double maxJumpSeconds = 8;

        public static double Tolerance = 0.75;

        public IList<Jump> Jumps { get; }

        public JumpDetectionUtility() {
            readings = new Queue<AccelerometerReading>();
            potentialJumpReadings = new List<AccelerometerReading>();
            started = false;
            jumpCount = 1;
            readingsSinceLastJump = 0;

            Jumps = new List<Jump>();
        }

        public void AddReading(AccelerometerReading reading) {
            if (!started && reading.Value < -startTolerance || reading.Value > startTolerance) {
                started = true;
            }

            if (!started) {
                return;
            }

            // set smoothed value
            if (lastReading != null) {
                reading.SmoothedValue = (reading.Value + lastReading.Value) / 2;
            } else {
                reading.SmoothedValue = reading.Value;
            }

            readings.Enqueue(reading);

            if (reading.SmoothedValue <= Tolerance && reading.SmoothedValue >= -Tolerance) {
                potentialJumpReadings.Add(reading);
            } else {
                double jumpTime = potentialJumpReadings.GetTime();

                if (jumpTime >= minJumpSeconds && jumpTime <= maxJumpSeconds) {
                    var jump = new Jump {
                        Number = jumpCount++,
                        Time = potentialJumpReadings.Select(j => j.Timestamp).Min(),
                        Airtime = potentialJumpReadings.GetTime(),
                        LandingGForce = 0,
                        Readings = potentialJumpReadings.ToList(),
                    };

                    Jumps.Add(jump);

                    lastJump = jump;

                    readingsSinceLastJump = 0;
                }

                potentialJumpReadings.Clear();
            }

            if (lastJump != null && readingsSinceLastJump == 4) {
                lastJump.LandingGForce = readings
                    .GetRange(readings.Count - 5, 4)
                    .Max(i => Math.Abs(i.SmoothedValue));
            }

            lastReading = reading;
            readingsSinceLastJump++;

            Debug.WriteLine(reading);

            // We don't want to store thousands of readings
            if (readings.Count > 10) {
                readings.Dequeue();
            }
        }
    }
}
