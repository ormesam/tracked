using System.Linq;
using Api.Analysers;
using Api.Utility;
using DataAccess;
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
        private readonly DbFactory dbFactory;

        public RidesController(DbFactory dbFactory) {
            this.dbFactory = dbFactory;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<RideDto> Get(int id) {
            int userId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateReadOnlyTransaction()) {
                var ride = RideHelper.GetRideDto(transaction, id, userId);

                if (ride == null) {
                    return NotFound();
                }

                return ride;
            }
        }

        [HttpPost]
        [Route("add")]
        public ActionResult<RideFeedDto> Add(CreateRideDto model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            int userId = this.GetCurrentUserId();
            int rideId;

            using (Transaction transaction = dbFactory.CreateTransaction()) {
                rideId = SaveRide(transaction, userId, model);

                Analyser.AnalyseRide(transaction, userId, rideId);

                transaction.Commit();
            }

            using (Transaction transaction = dbFactory.CreateReadOnlyTransaction()) {
                return GetRideOverview(transaction, rideId);
            }
        }

        private RideFeedDto GetRideOverview(Transaction transaction, int rideId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                var medals = context.TrailAttempts
                    .Where(row => row.RideId == rideId)
                    .Where(row => row.Medal != (int)Medal.None)
                    .Select(row => (Medal)row.Medal)
                    .ToList();

                return context.Rides
                    .Where(row => row.RideId == rideId)
                    .Select(row => new RideFeedDto {
                        UserId = row.UserId,
                        UserName = row.User.Name,
                        UserProfileImageUrl = row.User.ProfileImageUrl,
                        RideId = row.RideId,
                        Name = row.Name,
                        Date = row.StartUtc,
                        DistanceMiles = row.DistanceMiles,
                        EndUtc = row.EndUtc,
                        MaxSpeedMph = row.MaxSpeedMph,
                        Medals = medals,
                        RouteSvgPath = row.RouteSvgPath,
                    })
                    .SingleOrDefault();
            }
        }

        private int SaveRide(Transaction transaction, int userId, CreateRideDto model) {
            var routeSvgPath = new SvgBuilder(model.Locations.Cast<ILatLng>()).Build();

            using (ModelDataContext context = transaction.CreateDataContext()) {
                Ride ride = new Ride();
                ride.StartUtc = model.StartUtc;
                ride.EndUtc = model.EndUtc;
                ride.UserId = userId;
                ride.AverageSpeedMph = model.Locations.Average(i => i.Mph);
                ride.MaxSpeedMph = model.Locations.Max(i => i.Mph);
                ride.DistanceMiles = DistanceHelpers.GetDistanceMile(model.Locations.Cast<ILatLng>().ToList());
                ride.RouteSvgPath = routeSvgPath;
                ride.AnalyserVersion = Analyser.AnalyserVersion;

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
        }

        [HttpPost]
        [Route("reanalyse")]
        public ActionResult Analyse([FromBody] int rideId) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            int userId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateTransaction()) {
                RideHelper.ThrowIfNotOwner(transaction, rideId, userId);

                using (ModelDataContext context = transaction.CreateDataContext()) {
                    var attempts = context.TrailAttempts.Where(row => row.RideId == rideId);
                    var jumpAchievements = context.UserJumpAchievements.Where(row => row.RideId == rideId);
                    var speedAchievements = context.UserSpeedAchievements.Where(row => row.RideId == rideId);
                    var distanceAchievements = context.UserDistanceAchievements.Where(row => row.RideId == rideId);

                    context.SaveChanges();
                }

                Analyser.AnalyseRide(transaction, userId, rideId);

                transaction.Commit();
            }

            return Ok();
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult<bool> Delete([FromBody] int rideId) {
            int userId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
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
                }

                transaction.Commit();
            }

            return true;
        }

        [HttpGet]
        [Route("latest-analyser-version")]
        public ActionResult<int> LatestAnalyserVersion() {
            return Analyser.AnalyserVersion;
        }
    }
}