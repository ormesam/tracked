using System;
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
    public class UsersController : ControllerBase {
        private readonly ModelDataContext context;

        public UsersController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        [Route("profile")]
        public ActionResult<ProfileDto> Profile() {
            int userId = this.GetCurrentUserId();

            var profile = context.User
                .Where(row => row.UserId == userId)
                .Select(row => new ProfileDto {
                    UserId = row.UserId,
                    Name = row.Name,
                    CreatedUtc = row.CreatedUtc,
                    Bio = row.Bio,
                    ProfileImageUrl = row.ProfileImageUrl,
                })
                .SingleOrDefault();

            if (profile == null) {
                return NotFound();
            }

            profile.LongestAirtime = GetLongestAirtime(userId);
            profile.MilesTravelled = GetMilesTravelled(userId);
            profile.MilesTravelledThisMonth = GetMilesTravelled(userId, DateTime.UtcNow.AddMonths(-1));
            profile.TopSpeedMph = GetTopSpeedMph(userId);

            return profile;
        }

        [HttpPost]
        [Route("update-bio")]
        public ActionResult UpdateBio(BioChangeDto bioModel) {
            int userId = this.GetCurrentUserId();

            var user = context.User.Find(userId);

            if (user == null) {
                return NotFound();
            }

            user.Bio = bioModel.Bio;

            context.SaveChanges();

            return Ok();
        }

        private double? GetLongestAirtime(int userId) {
            return context.Jump
                .Where(row => row.Ride.UserId == userId)
                .Max(i => (double?)i.Airtime);
        }

        private double? GetMilesTravelled(int userId, DateTime? dateTime = null) {
            var query = context.Ride
                .Where(row => row.UserId == userId);

            if (dateTime != null) {
                query = query
                    .Where(row => row.StartUtc > dateTime.Value.Date);
            }

            return query.Sum(i => (double?)i.DistanceMiles);
        }

        private double? GetTopSpeedMph(int userId) {
            return context.Ride
                .Where(row => row.UserId == userId)
                .Max(i => (double?)i.MaxSpeedMph);
        }
    }
}
