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
                .OrderBy(row => row.RideLocation.Timestamp)
                .Select(row => new RideLocationDto {
                    AccuracyInMetres = row.RideLocation.AccuracyInMetres,
                    Altitude = row.RideLocation.Altitude,
                    Latitude = row.RideLocation.Latitude,
                    Longitude = row.RideLocation.Longitude,
                    Mph = row.RideLocation.Mph,
                    Timestamp = row.RideLocation.Timestamp,
                })
                .ToList();

            segmentAttempt.Jumps = context.SegmentAttemptJump
                .Where(row => row.SegmentAttemptId == id)
                .OrderBy(row => row.Number)
                .Select(row => new JumpDto {
                    JumpId = row.JumpId,
                    Airtime = row.Jump.Airtime,
                    Number = row.Number,
                    Timestamp = row.Jump.Timestamp,
                })
                .ToList();

            return segmentAttempt;
        }
    }
}
