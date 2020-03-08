using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Dtos;
using Tracked.Accelerometer;
using Tracked.Contexts;
using Tracked.JumpDetection;
using Tracked.Models;

namespace Tracked.Utilities {
    public class RideRecorder {
        private readonly MainContext context;
        private readonly bool detectJumps;
        private readonly IList<AccelerometerReadingDto> readings;
        private readonly JumpDetectionUtility jumpDetectionUtility;

        public readonly RideUploadDto Ride;

        public RideRecorder(MainContext context) {
            this.context = context;
            this.detectJumps = context.Settings.DetectJumps;
            Ride = new RideUploadDto();
            readings = new List<AccelerometerReadingDto>();
            jumpDetectionUtility = new JumpDetectionUtility(GeoUtility.Instance);
        }

        public async Task StartRide() {
            Ride.StartUtc = DateTime.UtcNow;

            AccelerometerUtility.Instance.AccelerometerChanged += AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged += GeoUtility_LocationChanged;

            await GeoUtility.Instance.Start();

            if (detectJumps) {
                await AccelerometerUtility.Instance.Start();
            }
        }

        public async Task StopRide() {
            Ride.EndUtc = DateTime.UtcNow;

            await GeoUtility.Instance.Stop();
            await AccelerometerUtility.Instance.Stop();

            AccelerometerUtility.Instance.AccelerometerChanged -= AccelerometerUtility_AccelerometerChanged;
            GeoUtility.Instance.LocationChanged -= GeoUtility_LocationChanged;

            if (detectJumps) {
                Ride.AccelerometerReadings = readings;
                Ride.Jumps = jumpDetectionUtility.Jumps;
            }

            await Model.Instance.SaveRideUpload(Ride);

            try {
                await context.Services.UploadRide(Ride);
                await Model.Instance.RemoveUploadRide(Ride);
            } catch (ServiceException ex) {
                Toast.LongAlert(ex.Message);
            }
        }

        private void AccelerometerUtility_AccelerometerChanged(AccelerometerChangedEventArgs e) {
            jumpDetectionUtility.AddReading(e.Data);
            readings.Add(e.Data);
        }

        private void GeoUtility_LocationChanged(LocationChangedEventArgs e) {
            Ride.Locations.Add(e.Location);
        }
    }
}
