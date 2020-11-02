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
                .OrderBy(row => row.Name)
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
                .OrderBy(row => row.Order)
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
                .OrderByDescending(row => row.StartUtc)
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

        [HttpPost]
        [Route("add")]
        public ActionResult<int> Add(SegmentDto model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            int userId = this.GetCurrentUserId();

            int segmentId = SaveSegment(userId, model);
            new SegmentAnalyser().AnalyseSegment(context, userId, segmentId);

            return segmentId;
        }

        private int SaveSegment(int userId, SegmentDto model) {
            Segment segment = new Segment {
                Name = model.Name,
                UserId = userId,
            };

            segment.SegmentLocation = model.Locations
                .Select(i => new SegmentLocation {
                    Latitude = i.Latitude,
                    Longitude = i.Longitude,
                    Order = i.Order,
                })
                .ToList();

            context.Segment.Add(segment);

            context.SaveChanges();

            return segment.SegmentId;
        }

        [HttpPost]
        [Route("change-name")]
        public ActionResult<string> ChangeName(SegmentChangeNameDto model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            int userId = this.GetCurrentUserId();

            var segment = context.Segment
                .Where(i => i.UserId == userId)
                .Where(i => i.SegmentId == model.SegmentId)
                .SingleOrDefault();

            if (segment == null) {
                return BadRequest();
            }

            segment.Name = model.Name;

            context.SaveChanges();

            return segment.Name;
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult<bool> Delete([FromBody] int segmentId) {
            int userId = this.GetCurrentUserId();

            var segment = context.Segment
                .Where(row => row.SegmentId == segmentId)
                .Where(row => row.UserId == userId)
                .SingleOrDefault();

            if (segment == null) {
                return NotFound();
            }

            var segmentLocations = context.SegmentLocation.Where(row => row.SegmentId == segmentId);
            var attempts = context.SegmentAttempt.Where(row => row.SegmentId == segmentId);
            var attemptLocations = context.SegmentAttemptLocation.Where(row => row.SegmentAttempt.SegmentId == segmentId);
            var attemptJumps = context.SegmentAttemptJump.Where(row => row.SegmentAttempt.SegmentId == segmentId);

            context.SegmentLocation.RemoveRange(segmentLocations);
            context.SegmentAttemptLocation.RemoveRange(attemptLocations);
            context.SegmentAttemptJump.RemoveRange(attemptJumps);
            context.SegmentAttempt.RemoveRange(attempts);
            context.Segment.Remove(segment);

            context.SaveChanges();

            return true;
        }
    }
}
