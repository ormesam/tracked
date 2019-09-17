using System.Collections.Generic;
using System.Linq;
using MtbMate.Models;

namespace MtbMate.Utilities {
    public class JumpDetectionUtility {
        private Ride ride;
        private double tolerance = 0.75;
        private double startTolerance = 2;
        private double minJumpSeconds = 0.5;
        private double maxJumpSeconds = 8;

        public JumpDetectionUtility(Ride ride) {
            this.ride = ride;
        }

        public void Run() {
            ride.Jumps.Clear();

            SmoothReadings();
            ConvertReadings();
            AnalyseReadings();
        }

        private void SmoothReadings() {
            for (int i = 1; i < ride.AccelerometerReadings.Count; i++) {
                ride.AccelerometerReadings[i].Value = (ride.AccelerometerReadings[i - 1].Value + ride.AccelerometerReadings[i].Value) / 2;
            }
        }

        private void ConvertReadings() {
            foreach (var reading in ride.AccelerometerReadings) {
                if (reading.Value < 0) {
                    reading.Value = -reading.Value;
                }
            }
        }

        private void AnalyseReadings() {
            IList<AccelerometerReading> potentialJumpReadings = new List<AccelerometerReading>();
            bool started = false;

            for (int i = 0; i < ride.AccelerometerReadings.Count; i++) {
                var reading = ride.AccelerometerReadings[i];

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
                        if (i + 4 <= ride.AccelerometerReadings.Count) {
                            double maxReading = ride.AccelerometerReadings
                                .GetRange(i, 4)
                                .Max(v => v.Value);

                            ride.Jumps.Add(new Jump {
                                Time = potentialJumpReadings.Select(j => j.Timestamp).Min(),
                                Airtime = potentialJumpReadings.GetTime(),
                                LandingGForce = maxReading,
                            });
                        }
                    }

                    potentialJumpReadings.Clear();
                }
            }
        }
    }
}
