using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MtbMate.JumpDetectionTests {
    class Program {
        static string file = "test2.csv";
        static double tolerance = 0.8;
        static double startTolerance = 2;
        static double minJumpSeconds = 0.3;
        static double maxJumpSeconds = 8;
        static List<Reading> readings = new List<Reading>();
        static IList<Jump> jumps = new List<Jump>();

        static void Main(string[] args) {
            PopulateReadings();
            PrintReadings();
            ConvertReadings();
            SmoothReadings();
            AnalyseReadings();
            PrintJumps();

            Console.WriteLine("Done...");
            Console.ReadLine();
        }

        private static void PrintJumps() {
            foreach (var jump in jumps) {
                Console.WriteLine($"Jump Detected: {jump.Readings.First().Date} for {jump.Readings.GetTime()} seconds with {Math.Round(jump.LandingReading, 1)}g landing force");
            }
        }

        private static void SmoothReadings() {
            for (int i = 1; i < readings.Count; i++) {
                readings[i].Value = (readings[i - 1].Value + readings[i].Value) / 2;
            }
        }

        private static void ConvertReadings() {
            foreach (var reading in readings) {
                if (reading.Value < 0) {
                    reading.Value = -reading.Value;
                }
            }
        }

        private static void AnalyseReadings() {
            IList<Reading> potentialJumpReadings = new List<Reading>();
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

                            jumps.Add(new Jump(potentialJumpReadings.ToList(), maxReading));
                        }
                    }

                    potentialJumpReadings.Clear();
                }
            }
        }

        private static void PrintReadings() {
            foreach (var reading in readings) {
                Console.WriteLine(reading.Date.ToString("dd/MM/yyyy HH:mm:ss.fff") + "  " + reading.Value);
            }
        }

        private static void PopulateReadings() {
            string[] allLines = File.ReadAllLines(file);

            // first row is header row
            for (int i = 1; i < allLines.Length; i++) {
                string[] line = allLines[i].Split(',');

                if (line.Length < 2) {
                    continue;
                }

                readings.Add(new Reading(DateTime.Parse(line[0]), double.Parse(line[1])));
            }

            readings = readings
                .OrderBy(i => i.Date)
                .ToList();
        }
    }

    class Reading {
        public DateTime Date { get; set; }
        public double Value { get; set; }

        public Reading(DateTime date, double value) {
            Date = date;
            Value = value;
        }
    }

    class Jump {
        public IList<Reading> Readings { get; set; }
        public double LandingReading { get; set; }

        public Jump(IList<Reading> readings, double landingReading) {
            Readings = readings;
            LandingReading = landingReading;
        }
    }

    static class Extensions {
        public static double GetTime(this IList<Reading> readings) {
            if (!readings.Any()) {
                return 0;
            }

            return (readings.Select(i => i.Date).Max() - readings.Select(i => i.Date).Min()).TotalSeconds;
        }
    }
}
