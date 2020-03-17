using System.Collections.Generic;
using System.Linq;
using Api.Analysers;
using Api.Utility;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Dtos;

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
                    Medals = medalsByRide[row.RideId],
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
                    SpeedMetresPerSecond = row.SpeedMetresPerSecond,
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

            int rideId = SaveRide(userId, model);
            SegmentAnalyser.AnalyseRide(context, userId, rideId);
            AchievementAnalyser.AnalyseRide(context, rideId, userId, model);

            return GetRideOverview(rideId);
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
                    Medals = medals,
                })
                .SingleOrDefault();
        }

        private int SaveRide(int userId, CreateRideDto model) {
            Ride ride = new Ride();
            ride.StartUtc = model.StartUtc;
            ride.EndUtc = model.EndUtc;
            ride.UserId = userId;

            ride.RideLocation = model.Locations
                .Select(i => new RideLocation {
                    AccuracyInMetres = i.AccuracyInMetres,
                    Altitude = i.Altitude,
                    Latitude = i.Latitude,
                    Longitude = i.Longitude,
                    SpeedMetresPerSecond = i.SpeedMetresPerSecond,
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
            var jumpAchievements = context.UserJumpAchievement.Where(row => row.RideId == rideId);
            var speedAchievements = context.UserSpeedAchievement.Where(row => row.RideId == rideId);

            context.RideLocation.RemoveRange(locations);
            context.Jump.RemoveRange(jumps);
            context.SegmentAttempt.RemoveRange(attempts);
            context.UserJumpAchievement.RemoveRange(jumpAchievements);
            context.UserSpeedAchievement.RemoveRange(speedAchievements);
            context.Ride.Remove(ride);

            context.SaveChanges();

            return true;
        }
    }
}