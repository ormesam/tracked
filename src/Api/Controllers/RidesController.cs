using System.Collections.Generic;
using System.Linq;
using Api.Analysers;
using Api.Utility;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Dtos;
using Shared.Interfaces;

namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RidesController : ControllerBase {
        private readonly ModelDataContext context;

        public RidesController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IList<RideOverviewDto>> Get() {
            int userId = this.GetCurrentUserId();

            var medalsByRide = context.SegmentAttempt
                .Where(row => row.UserId == userId)
                .Where(row => row.Medal != (int)Medal.None)
                .ToLookup(row => row.RideId, row => (Medal)row.Medal);

            var rides = context.Ride
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
                    RouteCanvasWidthSvg = row.RouteCanvasWidthSvg,
                    RouteCanvasHeightSvg = row.RouteCanvasHeightSvg,
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

            var ride = context.Ride
                .Where(row => row.UserId == userId)
                .Where(row => row.RideId == id)
                .Select(row => new RideDto {
                    RideId = row.RideId,
                    StartUtc = row.StartUtc,
                    EndUtc = row.EndUtc,
                    Name = row.Name,
                    AverageSpeedMph = row.AverageSpeedMph,
                    MaxSpeedMph = row.MaxSpeedMph,
                    DistanceMiles = row.DistanceMiles,
                })
                .SingleOrDefault();

            if (ride == null) {
                return NotFound();
            }

            ride.Jumps = context.Jump
                .Where(row => row.RideId == id)
                .OrderBy(row => row.Number)
                .Select(row => new JumpDto {
                    JumpId = row.JumpId,
                    Airtime = row.Airtime,
                    Number = row.Number,
                    Timestamp = row.Timestamp,
                })
                .ToList();

            ride.Locations = context.RideLocation
                .Where(row => row.RideId == id)
                .OrderBy(row => row.Timestamp)
                .Select(row => new RideLocationDto {
                    RideLocationId = row.RideLocationId,
                    AccuracyInMetres = row.AccuracyInMetres,
                    Altitude = row.Altitude,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    Mph = row.Mph,
                    Timestamp = row.Timestamp,
                })
                .ToList();

            ride.SegmentAttempts = context.SegmentAttempt
                .Where(row => row.RideId == id)
                .Select(row => new SegmentAttemptOverviewDto {
                    SegmentAttemptId = row.SegmentAttemptId,
                    RideId = row.RideId,
                    DisplayName = row.Segment.Name,
                    StartUtc = row.StartUtc,
                    EndUtc = row.EndUtc,
                    Medal = (Medal)row.Medal,
                })
                .ToList()
                .OrderByDescending(row => row.Time)
                .ToList();

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
            SegmentAnalyser.AnalyseRide(context, userId, model.RideId.Value);
            AchievementAnalyser.AnalyseRide(context, userId, model);

            return GetRideOverview(model.RideId.Value);
        }

        private RideOverviewDto GetRideOverview(int rideId) {
            var medals = context.SegmentAttempt
                .Where(row => row.RideId == rideId)
                .Where(row => row.Medal != (int)Medal.None)
                .Select(row => (Medal)row.Medal);

            return context.Ride
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
                    RouteCanvasWidthSvg = row.RouteCanvasWidthSvg,
                    RouteCanvasHeightSvg = row.RouteCanvasHeightSvg,
                })
                .SingleOrDefault();
        }

        private int SaveRide(int userId, CreateRideDto model) {
            var routeSvgDetails = new SvgBuilder(model.Locations.Cast<ILatLng>()).Build();

            Ride ride = new Ride();
            ride.StartUtc = model.StartUtc;
            ride.EndUtc = model.EndUtc;
            ride.UserId = userId;
            ride.AverageSpeedMph = model.Locations.Average(i => i.Mph);
            ride.MaxSpeedMph = model.Locations.Max(i => i.Mph);
            ride.DistanceMiles = DistanceHelpers.GetDistanceMile(model.Locations.Cast<ILatLng>().ToList());
            ride.RouteCanvasWidthSvg = routeSvgDetails.width;
            ride.RouteCanvasHeightSvg = routeSvgDetails.height;
            ride.RouteSvgPath = routeSvgDetails.path;

            ride.RideLocation = model.Locations
                .Select(i => new RideLocation {
                    AccuracyInMetres = i.AccuracyInMetres,
                    Altitude = i.Altitude,
                    Latitude = i.Latitude,
                    Longitude = i.Longitude,
                    Mph = i.Mph,
                    Timestamp = i.Timestamp,
                })
                .ToList();

            ride.Jump = model.Jumps
                .Select(i => new Jump {
                    Airtime = i.Airtime,
                    Number = i.Number,
                    Timestamp = i.Timestamp,
                })
                .ToList();

            ride.AccelerometerReading = model.AccelerometerReadings
                .Select(i => new AccelerometerReading {
                    Time = i.Timestamp,
                    X = i.X,
                    Y = i.Y,
                    Z = i.Z,
                })
                .ToList();

            context.Ride.Add(ride);

            context.SaveChanges();

            return ride.RideId;
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult<bool> Delete([FromBody] int rideId) {
            int userId = this.GetCurrentUserId();

            var ride = context.Ride
                .Where(row => row.RideId == rideId)
                .Where(row => row.UserId == userId)
                .SingleOrDefault();

            if (ride == null) {
                return NotFound();
            }

            var locations = context.RideLocation.Where(row => row.RideId == rideId);
            var jumps = context.Jump.Where(row => row.RideId == rideId);
            var attempts = context.SegmentAttempt.Where(row => row.RideId == rideId);
            var attemptLocations = context.SegmentAttemptLocation.Where(row => row.RideLocation.RideId == rideId);
            var attemptJumps = context.SegmentAttemptJump.Where(row => row.Jump.RideId == rideId);
            var jumpAchievements = context.UserJumpAchievement.Where(row => row.RideId == rideId);
            var speedAchievements = context.UserSpeedAchievement.Where(row => row.RideId == rideId);
            var distanceAchievements = context.UserDistanceAchievement.Where(row => row.RideId == rideId);

            context.RideLocation.RemoveRange(locations);
            context.Jump.RemoveRange(jumps);
            context.SegmentAttempt.RemoveRange(attempts);
            context.SegmentAttemptLocation.RemoveRange(attemptLocations);
            context.SegmentAttemptJump.RemoveRange(attemptJumps);
            context.UserJumpAchievement.RemoveRange(jumpAchievements);
            context.UserSpeedAchievement.RemoveRange(speedAchievements);
            context.UserDistanceAchievement.RemoveRange(distanceAchievements);
            context.Ride.Remove(ride);

            context.SaveChanges();

            return true;
        }
    }
}