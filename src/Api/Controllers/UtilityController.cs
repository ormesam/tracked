using System;
using System.Collections.Generic;
using System.Linq;
using Api.Utility;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
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
            var rideIds = context.Rides
                .Select(row => row.RideId)
                .ToList();

            foreach (var rideId in rideIds) {
                var ride = RideHelper.GetRideDto(context, rideId, null);

                if (!ride.Jumps.Any()) {
                    continue;
                }

                ProcessJumps(ride.Locations, ride.Jumps);
            }


            return "Done";
        }

        // Populate existing data - Very temporary
        private void ProcessJumps(IList<RideLocationDto> rideLocations, IList<JumpDto> rideJumps) {
            var locations = rideLocations
                .OrderBy(i => i.Timestamp)
                .Select(i => new {
                    i.Timestamp,
                    i.Latitude,
                    i.Longitude,
                    i.Mph,
                })
                .ToList();

            var jumpsByLocationTime = new Dictionary<DateTime, JumpDto>();

            foreach (var jump in rideJumps) {
                var nearestLocation = locations
                    .OrderBy(i => Math.Abs((i.Timestamp - jump.Timestamp).TotalSeconds))
                    .FirstOrDefault();

                if (!jumpsByLocationTime.ContainsKey(nearestLocation.Timestamp)) {
                    jumpsByLocationTime.Add(nearestLocation.Timestamp, jump);
                }
            }

            foreach (var location in locations) {
                var temp = new {
                    Jump = jumpsByLocationTime.ContainsKey(location.Timestamp) ? jumpsByLocationTime[location.Timestamp] : null,
                    Mph = location.Mph,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                };

                if (temp.Jump != null) {
                    var jump = context.Jumps.Find(temp.Jump.JumpId);
                    jump.Latitude = temp.Latitude;
                    jump.Longitude = temp.Longitude;
                    context.SaveChanges();
                }
            }
        }
    }
}
