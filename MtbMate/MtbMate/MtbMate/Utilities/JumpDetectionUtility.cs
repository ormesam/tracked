using System.Collections.Generic;
using System.Linq;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class JumpDetectionUtility {
        private readonly IList<AccelerometerReading> readings;
        private const double tolerance = 0.75;
        private const double startTolerance = 2;
        private const double minJumpSeconds = 0.5;
        private const double maxJumpSeconds = 8;

        public JumpDetectionUtility(IList<AccelerometerReading> readings) {
            this.readings = readings;
        }

        public IList<Jump> Run() {
            SmoothReadings();
            ConvertReadings();
            return AnalyseReadings();
        }

        private void SmoothReadings() {
            for (int i = 1; i < readings.Count; i++) {
                readings[i].Value = (readings[i - 1].Value + readings[i].Value) / 2;
            }
        }

        private void ConvertReadings() {
            foreach (var reading in readings) {
                if (reading.Value < 0) {
                    reading.Value = -reading.Value;
                }
            }
        }

        private IList<Jump> AnalyseReadings() {
            IList<Jump> jumps = new List<Jump>();
            IList<AccelerometerReading> potentialJumpReadings = new List<AccelerometerReading>();
            bool started = false;

            for (int i = 0; i < readings.Count; i++) {
                var reading = readings[i];

                if (!started && reading.Value >= startTolerance) {
                    started = true;
                }

                if (!started) {
                    continue;
                }

                if (reading.Value <= tolerance) {
                    potentialJumpReadings.Add(reading);
                } else {
                    double jumpTime = potentialJumpReadings.GetTime();

                    if (jumpTime >= minJumpSeconds && jumpTime <= maxJumpSeconds) {
                        if (i + 4 <= readings.Count) {
                            double maxReading = readings
                                .GetRange(i, 4)
                                .Max(v => v.Value);

                            jumps.Add(new Jump {
                                Time = potentialJumpReadings.Select(j => j.Timestamp).Min(),
                                Airtime = potentialJumpReadings.GetTime(),
                                LandingGForce = maxReading,
                            });
                        }
                    }

                    potentialJumpReadings.Clear();
                }
            }

            return jumps;
        }
    }
}
