using System.Collections.Generic;
using System.Linq;
using Api.Analysers;
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
    public class TrailsController : ControllerBase {
        private readonly ModelDataContext context;

        public TrailsController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IList<TrailOverviewDto>> Get() {
            var trails = context.Trail
                .OrderBy(row => row.Name)
                .Select(row => new TrailOverviewDto {
                    TrailId = row.TrailId,
                    Name = row.Name,
                })
                .ToList();

            return trails;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<TrailDto> Get(int id) {
            var trail = context.Trail
                .Where(row => row.TrailId == id)
                .Select(row => new TrailDto {
                    TrailId = row.TrailId,
                    Name = row.Name,
                })
                .SingleOrDefault();

            if (trail == null) {
                return NotFound();
            }

            trail.Locations = context.TrailLocation
                .Where(row => row.TrailId == id)
                .OrderBy(row => row.Order)
                .Select(row => new TrailLocationDto {
                    TrailLocationId = row.TrailLocationId,
                    TrailId = row.TrailId,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    Order = row.Order,
                })
                .ToList();

            trail.Attempts = context.TrailAttempt
                .Where(row => row.TrailId == id)
                .OrderByDescending(row => row.StartUtc)
                .Select(row => new TrailAttemptDto {
                    TrailAttemptId = row.TrailAttemptId,
                    RideId = row.RideId,
                    DisplayName = row.Ride.Name ?? row.Ride.StartUtc.ToString("dd MMM yy HH:mm"),
                    StartUtc = row.StartUtc,
                    EndUtc = row.EndUtc,
                    Medal = (Medal)row.Medal,
                })
                .ToList();

            return trail;
        }

        [HttpPost]
        [Route("add")]
        public ActionResult<int> Add(TrailDto model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!this.IsCurrentUserAdmin()) {
                return NotFound();
            }

            int userId = this.GetCurrentUserId();

            int trailId = SaveTrail(userId, model);
            new TrailAnalyser().AnalyseTrail(context, userId, trailId);

            return trailId;
        }

        private int SaveTrail(int userId, TrailDto model) {
            Trail trail = new Trail {
                Name = model.Name,
                UserId = userId,
            };

            trail.TrailLocation = model.Locations
                .Select(i => new TrailLocation {
                    Latitude = i.Latitude,
                    Longitude = i.Longitude,
                    Order = i.Order,
                })
                .ToList();

            context.Trail.Add(trail);

            context.SaveChanges();

            return trail.TrailId;
        }

        [HttpPost]
        [Route("change-name")]
        public ActionResult<string> ChangeName(TrailChangeNameDto model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (!this.IsCurrentUserAdmin()) {
                return NotFound();
            }

            var trail = context.Trail
                .Where(i => i.TrailId == model.TrailId)
                .SingleOrDefault();

            if (trail == null) {
                return BadRequest();
            }

            trail.Name = model.Name;

            context.SaveChanges();

            return trail.Name;
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult<bool> Delete([FromBody] int trailId) {
            if (!this.IsCurrentUserAdmin()) {
                return NotFound();
            }

            var trail = context.Trail
                .Where(row => row.TrailId == trailId)
                .SingleOrDefault();

            if (trail == null) {
                return NotFound();
            }

            var trailLocations = context.TrailLocation.Where(row => row.TrailId == trailId);
            var attempts = context.TrailAttempt.Where(row => row.TrailId == trailId);

            context.TrailLocation.RemoveRange(trailLocations);
            context.TrailAttempt.RemoveRange(attempts);
            context.Trail.Remove(trail);

            context.SaveChanges();

            return true;
        }
    }
}
