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

            ride.Jumps = context.RideJump
                .Where(row => row.RideId == id)
                .Select(row => new RideJumpDto {
                    RideJumpId = row.RideJumpId,
                    RideId = row.RideId,
                    Airtime = row.Airtime,
                    Number = row.Number,
                    Timestamp = row.Timestamp,
                })
                .ToList();

            ride.Locations = context.RideLocation
                .Where(row => row.RideId == id)
                .Select(row => new RideLocationDto {
                    RideLocationId = row.RideLocationId,
                    RideId = row.RideId,
                    AccuracyInMetres = row.AccuracyInMetres,
                    Altitude = row.Altitude,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    SpeedMetresPerSecond = row.SpeedMetresPerSecond,
                    Timestamp = row.Timestamp,
                })
                .ToList();

            ride.SegmentAttempts = context.SegmentAttempt
                .Where(row => row.RideId == row.RideId)
                .Select(row => new SegmentAttemptOverviewDto {
                    SegmentAttemptId = row.SegmentAttemptId,
                    RideId = row.RideId,
                    DisplayName = row.Segment.Name,
                    StartUtc = row.StartUtc,
                    EndUtc = row.EndUtc,
                    Medal = (Medal)row.Medal,
                })
                .ToList();

            return ride;
        }

        [HttpPost]
        [Route("add")]
        public ActionResult<int> Add(RideUploadDto model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            int userId = this.GetCurrentUserId();

            int rideId = SaveRide(userId, model);
            AnalyseAndSaveSegmentAttempts(rideId, userId, model);

            return rideId;
        }

        private void AnalyseAndSaveSegmentAttempts(int rideId, int userId, RideUploadDto model) {
            var matchingSegments = SegmentAnalyser.GetMatchingSegments(context, model.Locations.ToArray(), model.Jumps.ToArray());

            foreach (var match in matchingSegments) {
                SegmentAttempt attempt = new SegmentAttempt {
                    RideId = rideId,
                    SegmentId = match.SegmentId,
                    UserId = userId,
                    StartUtc = match.StartUtc,
                    EndUtc = match.EndUtc,
                    Medal = (int)match.Medal,
                    SegmentAttemptLocation = match.Locations
                        .Select(i => new SegmentAttemptLocation {
                            AccuracyInMetres = i.AccuracyInMetres,
                            Altitude = i.Altitude,
                            Latitude = i.Latitude,
                            Longitude = i.Longitude,
                            SpeedMetresPerSecond = i.SpeedMetresPerSecond,
                            Timestamp = i.Timestamp,
                        })
                        .ToList(),
                    SegmentAttemptJump = match.Jumps
                        .Select(i => new SegmentAttemptJump {
                            Airtime = i.Airtime,
                            Number = i.Number,
                            Timestamp = i.Timestamp,
                        })
                        .ToList(),
                };

                context.SegmentAttempt.Add(attempt);

                context.SaveChanges();
            }
        }

        private int SaveRide(int userId, RideUploadDto model) {
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

            ride.RideJump = model.Jumps
                .Select(i => new RideJump {
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
    }
}