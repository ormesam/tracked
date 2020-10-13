using System.Linq;
using Api.Utility;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Dtos;

/// <summary>
/// Used for upgrading existing data
/// </summary>
namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase {
        private ModelDataContext context;

        public UtilityController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<string> Get() {
            var rides = context.Ride.ToList();

            foreach (var ride in rides) {
                var r = Get(ride.RideId);

                var routeSvgDetails = new SvgBuilder(r).Build();
                ride.RouteCanvasWidthSvg = routeSvgDetails.width;
                ride.RouteCanvasHeightSvg = routeSvgDetails.height;
                ride.RouteSvgPath = routeSvgDetails.path;

                context.SaveChanges();
            }

            return "Done";
        }

        private RideDto Get(int id) {
            var ride = context.Ride
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
    }
}
