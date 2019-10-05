using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MtbMate.Models;
using MtbMate.Utilities;

namespace MtbMate.Tests.Jumps {
    public abstract class JumpTestBase {
        public Ride Ride;
        public JumpDetectionUtility JumpDetectionUtility;

        public abstract string FileName { get; }


        [TestInitialize]
        public void Initialize() {
            IList<AccelerometerReading> readings = new List<AccelerometerReading>();

            string[] allLines = File.ReadAllLines(FileName);

            // first row is header row
            for (int i = 1; i < allLines.Length; i++) {
                string[] line = allLines[i].Split(',');

                if (line.Length < 2) {
                    continue;
                }

                readings.Add(new AccelerometerReading {
                    Timestamp = DateTime.Parse(line[0]),
                    Value = double.Parse(line[1]),
                });
            }

            readings = readings
                .OrderBy(i => i.Timestamp)
                .ToList();

            Ride = new Ride();
            Ride.AccelerometerReadings = readings;

            JumpDetectionUtility = new JumpDetectionUtility(Ride);
        }
    }
}
