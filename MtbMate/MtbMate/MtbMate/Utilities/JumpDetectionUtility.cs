using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class JumpDetectionUtility {
        private readonly Queue<AccelerometerReading> readings;
        private readonly IList<AccelerometerReading> potentialJumpReadings;
        private AccelerometerReading lastReading;
        private bool started;
        private int jumpCount;
        private const double startTolerance = 2;
        private const double minJumpSeconds = 0.5;
        private const double maxJumpSeconds = 3;

        public static double Tolerance = 0.65;

        public IList<Jump> Jumps { get; }

        public JumpDetectionUtility() {
            readings = new Queue<AccelerometerReading>();
            potentialJumpReadings = new List<AccelerometerReading>();
            started = false;
            jumpCount = 1;

            Jumps = new List<Jump>();
        }

        public void AddReading(AccelerometerReading reading) {
            if (!started && reading.Value < -startTolerance || reading.Value > startTolerance) {
                started = true;
            }

            if (!started) {
                return;
            }

            Debug.WriteLine(reading);

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
                        Readings = potentialJumpReadings.ToList(),
                    };

                    Jumps.Add(jump);
                }

                potentialJumpReadings.Clear();
            }

            lastReading = reading;

            // We don't want to store thousands of readings
            if (readings.Count > 10) {
                readings.Dequeue();
            }
        }
    }
}
