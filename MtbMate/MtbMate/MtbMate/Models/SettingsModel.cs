using System;
using MtbMate.Accelerometer;

namespace MtbMate.Models
{
    public class SettingsModel
    {
        public Guid? Id { get; set; }
        public AccelerometerType AccelerometerType { get; set; }

        public void ResetDefaults()
        {
            AccelerometerType = AccelerometerType.Ble;
        }
    }
}
