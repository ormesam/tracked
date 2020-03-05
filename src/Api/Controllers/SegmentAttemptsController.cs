using System.Linq;
using Api.Utility;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Dtos;

namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SegmentAttemptsController : ControllerBase {
        private readonly ModelDataContext context;

        public SegmentAttemptsController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<SegmentAttemptDto> Get(int id) {
            int userId = this.GetCurrentUserId();

            var segmentAttempt = context.SegmentAttempt
                .Where(row => row.UserId == userId)
                .Where(row => row.SegmentAttemptId == id)
                .Select(row => new SegmentAttemptDto {
                    SegmentAttemptId = row.SegmentAttemptId,
                    RideId = row.RideId,
                    SegmentId = row.SegmentId,
                    StartUtc = row.StartUtc,
                    EndUtc = row.EndUtc,
                    Medal = (Medal)row.Medal,
                })
                .SingleOrDefault();

            if (segmentAttempt == null) {
                return NotFound();
            }

            segmentAttempt.Locations = context.SegmentAttemptLocation
                .Where(row => row.SegmentAttemptId == segmentAttempt.SegmentAttemptId)
                .Select(row => new SegmentAttemptLocationDto {
                    SegmentAttemptLocationId = row.SegmentAttemptLocationId,
                    SegmentAttemptId = row.SegmentAttemptId,
                    AccuracyInMetres = row.AccuracyInMetres,
                    Altitude = row.Altitude,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    SpeedMetresPerSecond = row.SpeedMetresPerSecond,
                    Timestamp = row.Timestamp,
                })
                .ToList();

            segmentAttempt.Jumps = context.SegmentAttemptJump
                .Where(row => row.SegmentAttemptId == id)
                .Select(row => new SegmentAttemptJumpDto {
                    SegmentAttemptJumpId = row.SegmentAttemptJumpId,
                    SegmentAttemptId = row.SegmentAttemptId,
                    Airtime = row.Airtime,
                    Number = row.Number,
                    Timestamp = row.Timestamp,
                })
                .ToList();

            return segmentAttempt;
        }
    }
}
