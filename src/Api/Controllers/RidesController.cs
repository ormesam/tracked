using System.Collections.Generic;
using System.Linq;
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
                    MaxSpeedMph = row.MaxSpeedMph,
                    AverageSpeedMph = row.AverageSpeedMph,
                    DistanceMiles = row.DistanceMiles,
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
    }
}