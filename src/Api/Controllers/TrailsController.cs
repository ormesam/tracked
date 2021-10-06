using System.Collections.Generic;
using System.Linq;
using Api.Analysers;
using Api.Utility;
using DataAccess;
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
        private readonly DbFactory dbFactory;

        public TrailsController(DbFactory dbFactory) {
            this.dbFactory = dbFactory;
        }

        [HttpGet]
        public ActionResult<IList<TrailOverviewDto>> Get() {
            using (Transaction transaction = dbFactory.CreateReadOnlyTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
                    var trails = context.Trails
                        .OrderBy(row => row.Name)
                        .Select(row => new TrailOverviewDto {
                            TrailId = row.TrailId,
                            Name = row.Name,
                        })
                        .ToList();

                    return trails;
                }
            }
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<TrailDto> Get(int id) {
            using (Transaction transaction = dbFactory.CreateReadOnlyTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
                    var trail = context.Trails
                        .Where(row => row.TrailId == id)
                        .Select(row => new TrailDto {
                            TrailId = row.TrailId,
                            Name = row.Name,
                        })
                        .SingleOrDefault();

                    if (trail == null) {
                        return NotFound();
                    }

                    trail.Locations = context.TrailLocations
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

                    trail.Attempts = context.TrailAttempts
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
            }
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
            int trailId;

            using (Transaction transaction = dbFactory.CreateTransaction()) {
                trailId = SaveTrail(transaction, userId, model);
                new TrailAnalyser().AnalyseTrail(transaction, userId, trailId);

                transaction.Commit();
            }

            return trailId;
        }

        private int SaveTrail(Transaction transaction, int userId, TrailDto model) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                Trail trail = new Trail {
                    Name = model.Name,
                    UserId = userId,
                };

                trail.TrailLocations = model.Locations
                    .Select(i => new TrailLocation {
                        Latitude = i.Latitude,
                        Longitude = i.Longitude,
                        Order = i.Order,
                    })
                    .ToList();

                context.Trails.Add(trail);

                context.SaveChanges();

                return trail.TrailId;
            }
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

            using (Transaction transaction = dbFactory.CreateTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
                    var trail = context.Trails
                        .Where(i => i.TrailId == model.TrailId)
                        .SingleOrDefault();

                    if (trail == null) {
                        return BadRequest();
                    }

                    trail.Name = model.Name;

                    context.SaveChanges();

                    transaction.Commit();

                    return trail.Name;
                }
            }
        }

        [HttpPost]
        [Route("delete")]
        public ActionResult<bool> Delete([FromBody] int trailId) {
            if (!this.IsCurrentUserAdmin()) {
                return NotFound();
            }

            using (Transaction transaction = dbFactory.CreateTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
                    var trail = context.Trails
                        .Where(row => row.TrailId == trailId)
                        .SingleOrDefault();

                    if (trail == null) {
                        return NotFound();
                    }

                    var trailLocations = context.TrailLocations.Where(row => row.TrailId == trailId);
                    var attempts = context.TrailAttempts.Where(row => row.TrailId == trailId);

                    context.TrailLocations.RemoveRange(trailLocations);
                    context.TrailAttempts.RemoveRange(attempts);
                    context.Trails.Remove(trail);

                    context.SaveChanges();
                }

                transaction.Commit();
            }

            return true;
        }
    }
}
