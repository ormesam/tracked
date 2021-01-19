using System.Collections.Generic;
using System.Linq;
using Api.Analysers;
using Api.Utility;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Dtos;
using Shared.Interfaces;

namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RidesController : ControllerBase {
        private readonly ModelDataContext context;

        public RidesController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IList<RideOverviewDto>> Get() {
            int userId = this.GetCurrentUserId();

            var medalsByRide = context.TrailAttempts
                .Where(row => row.UserId == userId)
                .Where(row => row.Medal != (int)Medal.None)
                .ToLookup(row => row.RideId, row => (Medal)row.Medal);

            var rides = context.Rides
                .Where(row => row.UserId == userId)
                .OrderByDescending(row => row.StartUtc)
                .Select(row => new RideOverviewDto {
                    RideId = row.RideId,
                    Name = row.Name,
                    StartUtc = row.StartUtc,
                    DistanceMiles = row.DistanceMiles,
                    EndUtc = row.EndUtc,
                    MaxSpeedMph = row.MaxSpeedMph,
                    RouteSvgPath = row.RouteSvgPath,
                    Medals = medalsByRide[row.RideId],
                    UserId = row.UserId,
                    UserName = row.User.Name,
                    UserProfileImageUrl = row.User.ProfileImageUrl,
                })
                .ToList();

            return rides;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<RideDto> Get(int id) {
            int userId = this.GetCurrentUserId();

            var ride = RideHelper.GetRideDto(context, id, userId);

            if (ride == null) {
                return NotFound();
            }

            return ride;
        }

        [HttpPost]
        [Route("add")]
        public ActionResult<RideOverviewDto> Add(CreateRideDto model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            int userId = this.GetCurrentUserId();

            model.RideId = SaveRide(userId, model);
            Analyser.AnalyseRide(context, userId, model);

            return GetRideOverview(model.RideId.Value);
        }

        private RideOverviewDto GetRideOverview(int rideId) {
            var medals = context.TrailAttempts
                .Where(row => row.RideId == rideId)
                .Where(row => row.Medal != (int)Medal.None)
                .Select(row => (Medal)row.Medal)
                .ToList();

            return context.Rides
                .Where(row => row.RideId == rideId)
                .Select(row => new RideOverviewDto {
                    RideId = row.RideId,
                    Name = row.Name,
                    StartUtc = row.StartUtc,
                    DistanceMiles = row.DistanceMiles,
                    EndUtc = row.EndUtc,
                    MaxSpeedMph = row.MaxSpeedMph,
                    Medals = medals,
                    UserId = row.UserId,
                    UserName = row.User.Name,
                    UserProfileImageUrl = row.User.ProfileImageUrl,
                    RouteSvgPath = row.RouteSvgPath,
                })
                .SingleOrDefault();
        }

        private int SaveRide(int userId, CreateRideDto model) {
            var routeSvgPath = new SvgBuilder(model.Locations.Cast<ILatLng>()).Build();

            Ride ride = new Ride();
            ride.StartUtc = model.StartUtc;
            ride.EndUtc = model.EndUtc;
            ride.UserId = userId;
            ride.AverageSpeedMph = model.Locations.Average(i => i.Mph);
            ride.MaxSpeedMph = model.Locations.Max(i => i.Mph);
            ride.DistanceMiles = DistanceHelpers.GetDistanceMile(model.Locations.Cast<ILatLng>().ToList());
            ride.RouteSvgPath = routeSvgPath;

            ride.RideLocations = model.Locations
                .Select(i => new RideLocation {
                    AccuracyInMetres = i.AccuracyInMetres,
                    Altitude = i.Altitude,
                    Latitude = i.Latitude,
                    Longitude = i.Longitude,
                    Mph = i.Mph,
                    Timestamp = i.Timestamp,
                })
                .ToList();

            ride.Jumps = model.Jumps
                .Select(i => new Jump {
                    Airtime = i.Airtime,
                    Number = i.Number,
                    Timestamp = i.Timestamp,
                    Latitude = i.Latitude,
                    Longitude = i.Longitude,
                })
                .ToList();

            ride.AccelerometerReadings = model.AccelerometerReadings
                .Select(i => new AccelerometerReading {
                    Time = i.Timestamp,
                    X = i.X,
                    Y = i.Y,
                    Z = i.Z,
                })
                .ToList();

            context.Rides.Add(ride);

            context.SaveChanges();

            return ride.RideId;
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult<bool> Delete([FromBody] int rideId) {
            int userId = this.GetCurrentUserId();

            var ride = context.Rides
                .Where(row => row.RideId == rideId)
                .Where(row => row.UserId == userId)
                .SingleOrDefault();

            if (ride == null) {
                return NotFound();
            }

            var locations = context.RideLocations.Where(row => row.RideId == rideId);
            var jumps = context.Jumps.Where(row => row.RideId == rideId);
            var attempts = context.TrailAttempts.Where(row => row.RideId == rideId);
            var jumpAchievements = context.UserJumpAchievements.Where(row => row.RideId == rideId);
            var speedAchievements = context.UserSpeedAchievements.Where(row => row.RideId == rideId);
            var distanceAchievements = context.UserDistanceAchievements.Where(row => row.RideId == rideId);
            var accelerometerReadings = context.AccelerometerReadings.Where(row => row.RideId == rideId);

            context.RideLocations.RemoveRange(locations);
            context.Jumps.RemoveRange(jumps);
            context.TrailAttempts.RemoveRange(attempts);
            context.UserJumpAchievements.RemoveRange(jumpAchievements);
            context.UserSpeedAchievements.RemoveRange(speedAchievements);
            context.UserDistanceAchievements.RemoveRange(distanceAchievements);
            context.AccelerometerReadings.RemoveRange(accelerometerReadings);
            context.Rides.Remove(ride);

            context.SaveChanges();

            return true;
        }
    }
}