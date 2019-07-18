﻿using System;

namespace MtbMate.Models
{
    public class AccelerometerReadingModel
    {
        public DateTime Timestamp { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override string ToString()
        {
            return $"{Timestamp} Reading: X: {X}, Y: {Y}, Z: {Z}";
        }
    }
}
