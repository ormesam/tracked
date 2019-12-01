using System.Collections.Generic;
using System.Linq;
using Api.Utility;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.Dtos;

namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SegmentsController : ControllerBase {
        private readonly ModelDataContext context;

        public SegmentsController(ModelDataContext context) {
            this.context = context;
        }

        [HttpPost]
        [Route("get")]
        public ActionResult<IList<SegmentDto>> GetExistingSegments(IList<int> existingSegmentIds) {
            int userId = this.GetCurrentUserId();

            var segments = context.Segment
                .Where(row => row.UserId == userId)
                .Where(row => !Enumerable.Contains(existingSegmentIds, row.SegmentId))
                .Select(row => new SegmentDto {
                    SegmentId = row.SegmentId,
                    Name = row.Name,
                })
                .ToList();

            var locationsBySegment = context.SegmentLocation
                .Where(row => row.Segment.UserId == userId)
                .Where(row => !Enumerable.Contains(existingSegmentIds, row.SegmentId))
                .Select(row => new SegmentLocationDto {
                    SegmentId = row.SegmentId,
                    Order = row.Order,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                })
                .ToLookup(i => i.SegmentId);

            var attemptsBySegment = context.SegmentAttempt
                .Where(row => row.Segment.UserId == userId)
                .Where(row => !Enumerable.Contains(existingSegmentIds, row.SegmentAttemptId))
                .Select(row => new SegmentAttemptDto {
                    SegmentAttemptId = row.SegmentAttemptId,
                    SegmentId = row.SegmentId,
                    RideId = row.RideId,
                    EndUtc = row.EndUtc,
                    Medal = (Medal?)row.Medal,
                    StartUtc = row.StartUtc
                })
                .ToLookup(i => i.SegmentId.Value);

            foreach (var segment in segments) {
                segment.Locations = locationsBySegment[segment.SegmentId.Value]
                    .OrderBy(i => i.Order)
                    .ToList();

                segment.Attempts = attemptsBySegment[segment.SegmentId.Value]
                    .OrderBy(i => i.StartUtc)
                    .ToList();
            }

            return segments;
        }


        [HttpPost]
        [Route("add")]
        public ActionResult<int> AddSegment(SegmentDto dto) {
            if (dto == null) {
                return BadRequest();
            }

            int userId = this.GetCurrentUserId();

            Segment segment = null;

            if (dto.SegmentId != null) {
                context.SegmentLocation.RemoveRange(context.SegmentLocation.Where(i => i.SegmentId == dto.SegmentId));

                context.SaveChanges();

                segment = context.Segment.SingleOrDefault(row => row.SegmentId == dto.SegmentId);
            }

            if (segment == null) {
                segment = new Segment();
                context.Segment.Add(segment);
            }

            segment.UserId = userId;
            segment.Name = dto.Name;

            segment.SegmentLocation = dto.Locations
                .Select(row => new SegmentLocation {
                    Order = row.Order,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                })
                .ToList();

            context.SaveChanges();

            return segment.SegmentId;
        }


        [HttpPost]
        [Route("add-attempt")]
        public ActionResult AddAttempt(SegmentAttemptDto dto) {
            if (dto == null) {
                return BadRequest();
            }

            int userId = this.GetCurrentUserId();

            if (CheckUserOwnsSegment(dto.SegmentId.Value, userId)) {
                return BadRequest();
            }

            SegmentAttempt attempt;

            if (dto.SegmentAttemptId == null) {
                attempt = new SegmentAttempt();
            } else {
                attempt = context.SegmentAttempt.SingleOrDefault(row => row.SegmentAttemptId == dto.SegmentAttemptId);
            }

            attempt.SegmentId = dto.SegmentId.Value;
            attempt.RideId = dto.RideId.Value;
            attempt.UserId = userId;
            attempt.EndUtc = dto.EndUtc;
            attempt.Medal = (int?)dto.Medal;
            attempt.StartUtc = dto.StartUtc;

            context.SaveChanges();

            return Ok();
        }

        private bool CheckUserOwnsSegment(int segmentId, int userId) {
            return context.Segment
                .Where(row => row.SegmentId == segmentId)
                .Where(row => row.UserId == userId)
                .Any();
        }
    }
}
