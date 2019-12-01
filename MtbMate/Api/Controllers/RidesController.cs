using System.Collections.Generic;
using System.Linq;
using Api.Utility;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RidesController : ControllerBase {
        private readonly ModelDataContext context;

        public RidesController(ModelDataContext context) {
            this.context = context;
        }

        [HttpPost]
        [Route("get")]
        public ActionResult<IList<RideDto>> AddRide(IList<int> existingRideIds) {
            int userId = this.GetCurrentUserId();

            var rides = context.Ride
                .Where(row => !Enumerable.Contains(existingRideIds, row.RideId))
                .OrderBy(row => row.StartUtc)
                .Select(row => new RideDto {
                    RideId = row.RideId,
                    Start = row.StartUtc,
                    End = row.EndUtc,
                })
                .ToList();

            var locationsByRide = context.RideLocation
                .Where(row => !Enumerable.Contains(existingRideIds, row.RideId))
                .Select(row => new LocationDto {
                    RideId = row.RideId,
                    AccuracyInMetres = row.AccuracyInMetres,
                    Altitude = row.Altitude,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    SpeedMetresPerSecond = row.SpeedMetresPerSecond,
                    Timestamp = row.Timestamp,
                })
                .ToLookup(i => i.RideId.Value);

            var jumpsByRide = context.Jump
                .Where(row => !Enumerable.Contains(existingRideIds, row.RideId))
                .Select(row => new JumpDto {
                    RideId = row.RideId,
                    Airtime = row.Airtime,
                    Number = row.Number,
                    Timestamp = row.Timestamp,
                })
                .ToLookup(i => i.RideId.Value);

            var accelerometerReadingsByJump = context.AccelerometerReading
                .Where(row => !Enumerable.Contains(existingRideIds, row.RideId))
                .Select(row => new AccelerometerReadingDto {
                    RideId = row.RideId,
                    Time = row.Time,
                    X = row.X,
                    Y = row.Y,
                    Z = row.Z,
                })
                .ToLookup(i => i.RideId.Value);

            foreach (var ride in rides) {
                ride.Locations = locationsByRide[ride.RideId.Value]
                    .OrderBy(i => i.Timestamp)
                    .ToList();

                ride.Jumps = jumpsByRide[ride.RideId.Value]
                    .OrderBy(i => i.Number)
                    .ToList();

                ride.AccelerometerReadings = accelerometerReadingsByJump[ride.RideId.Value]
                    .OrderBy(i => i.Time)
                    .ToList();
            }

            return rides;
        }

        [HttpPost]
        [Route("add")]
        public ActionResult<int> AddRide(RideDto dto) {
            if (dto == null) {
                return BadRequest();
            }

            int userId = this.GetCurrentUserId();

            Ride ride = null;

            if (dto.RideId != null) {

                context.RideLocation.RemoveRange(context.RideLocation.Where(i => i.RideId == dto.RideId));
                context.Jump.RemoveRange(context.Jump.Where(i => i.RideId == dto.RideId));
                context.AccelerometerReading.RemoveRange(context.AccelerometerReading.Where(i => i.RideId == dto.RideId));

                context.SaveChanges();

                ride = context.Ride.SingleOrDefault(row => row.RideId == dto.RideId);
            }

            if (ride == null) {
                ride = new Ride();
                context.Ride.Add(ride);
            }

            ride.UserId = userId;
            ride.StartUtc = dto.Start;
            ride.EndUtc = dto.End;

            ride.RideLocation = dto.Locations
                .Select(row => new RideLocation {
                    Timestamp = row.Timestamp,
                    AccuracyInMetres = row.AccuracyInMetres,
                    Altitude = row.Altitude,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    SpeedMetresPerSecond = row.SpeedMetresPerSecond,
                })
                .ToList();

            ride.Jump = dto.Jumps
                .Select(row => new Jump {
                    Airtime = row.Airtime,
                    Number = row.Number,
                    Timestamp = row.Timestamp,
                })
                .ToList();

            ride.AccelerometerReading = dto.AccelerometerReadings
                .Select(row => new AccelerometerReading {
                    Time = row.Time,
                    X = row.X,
                    Y = row.Y,
                    Z = row.Z,
                })
                .ToList();

            context.SaveChanges();

            return ride.RideId;
        }
    }
}
