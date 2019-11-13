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
                .Select(row => new RideDto {
                    RideId = row.RideId,
                    Start = row.StartUtc,
                    End = row.EndUtc,
                    Name = row.Name,
                })
                .ToList();

            var locationsByRide = context.Location
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
                    LandingGForce = row.LandingGforce,
                    Number = row.Number,
                    Time = row.Time,
                })
                .ToLookup(i => i.RideId.Value);

            foreach (var ride in rides) {
                ride.Locations = locationsByRide[ride.RideId.Value]
                    .OrderBy(i => i.Timestamp)
                    .ToList();

                ride.Jumps = jumpsByRide[ride.RideId.Value]
                    .OrderBy(i => i.Number)
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

                context.Location.RemoveRange(context.Location.Where(i => i.RideId == dto.RideId));
                context.Jump.RemoveRange(context.Jump.Where(i => i.RideId == dto.RideId));

                context.SaveChanges();

                ride = context.Ride.SingleOrDefault(row => row.RideId == dto.RideId);
            }

            if (ride == null) {
                ride = new Ride();
                context.Ride.Add(ride);
            }

            ride.UserId = userId;
            ride.Name = dto.Name;
            ride.StartUtc = dto.Start;
            ride.EndUtc = dto.End;

            ride.Location = dto.Locations
                .Select(row => new Location {
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
                    LandingGforce = row.LandingGForce,
                    Number = row.Number,
                    Time = row.Time,
                })
                .ToList();

            context.SaveChanges();

            return ride.RideId;
        }
    }
}
