using System.Linq;
using Api.Utility;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RidesController : ControllerBase {
        private readonly ModelDataContext context;

        public RidesController(ModelDataContext context) {
            this.context = context;
        }

        [HttpPost]
        [Route("add")]
        public ActionResult<int> AddRide(RideDto dto) {
            if (dto == null) {
                return BadRequest();
            }

            int userId = this.GetCurrentUserId();

            Ride ride = null;

            if (dto.RideId != null) {

                context.Location.RemoveRange(context.Location.Where(i => i.RideId == dto.RideId));
                context.Jump.RemoveRange(context.Jump.Where(i => i.RideId == dto.RideId));

                context.SaveChanges();

                ride = context.Ride.SingleOrDefault(row => row.RideId == dto.RideId);
            }

            if (ride == null) {
                ride = new Ride();
                context.Ride.Add(ride);
            }

            ride.UserId = userId;
            ride.Name = dto.Name;
            ride.StartUtc = dto.Start;
            ride.EndUtc = dto.End;
            ride.Location = dto.Locations
                .Select(row => new Location {
                    AccuracyInMetres = row.AccuracyInMetres,
                    Altitude = row.Altitude,
                    Latitude = row.Latitude,
                    Longitude = row.Longitude,
                    SpeedMetresPerSecond = row.SpeedMetresPerSecond,
                })
                .ToList();
            ride.Jump = dto.Jumps
                .Select(row => new Jump {
                    Airtime = row.Airtime,
                    LandingGforce = row.LandingGForce,
                    Number = row.Number,
                    Time = row.Time,
                })
                .ToList();

            context.SaveChanges();

            return ride.RideId;
        }
    }
}
