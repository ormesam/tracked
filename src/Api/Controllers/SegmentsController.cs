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
    public class SegmentsController : ControllerBase {
        private readonly ModelDataContext context;

        public SegmentsController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IList<SegmentOverviewDto>> Get() {
            int userId = this.GetCurrentUserId();

            var segments = context.Segment
                .Where(row => row.UserId == userId)
                .Select(row => new SegmentOverviewDto {
                    SegmentId = row.SegmentId,
                    Name = row.Name,
                })
                .ToList();

            return segments;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<SegmentDto> Get(int id) {
            int userId = this.GetCurrentUserId();

            var segment = context.Segment
                .Where(row => row.UserId == userId)
                .Where(row => row.SegmentId == id)
                .Select(row => new SegmentDto {
                    SegmentId = row.SegmentId,
                    Name = row.Name,
                })
                .SingleOrDefault();

            if (segment == null) {
                return NotFound();
            }

            segment.Locations = context.SegmentLocation
                .Where(row => row.SegmentId == id)
                .Select(row => new SegmentLocationDto {
                    SegmentLocationId = row.SegmentLocationId,
                    SegmentId = row.SegmentId,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    Order = row.Order,
                })
                .ToList();

            segment.Attempts = context.SegmentAttempt
                .Where(row => row.SegmentId == id)
                .Select(row => new SegmentAttemptOverviewDto {
                    SegmentAttemptId = row.SegmentAttemptId,
                    RideId = row.RideId,
                    DisplayName = row.Ride.Name ?? row.Ride.StartUtc.ToString("dd MMM yy HH:mm"),
                    StartUtc = row.StartUtc,
                    EndUtc = row.EndUtc,
                    Medal = (Medal)row.Medal,
                })
                .ToList();

            return segment;
        }
    }
}
