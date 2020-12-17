using System;
using System.Collections.Generic;
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

            var profile = context.Users
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
            profile.MilesTravelledThisMonth = GetMilesTravelled(userId, DateTime.UtcNow.AddDays(-30));
            profile.TopSpeedMph = GetTopSpeedMph(userId);
            profile.Achievements = GetAchievements(userId);

            return profile;
        }

        [HttpPost]
        [Route("update-bio")]
        public ActionResult UpdateBio(BioChangeDto bioModel) {
            int userId = this.GetCurrentUserId();

            var user = context.Users.Find(userId);

            if (user == null) {
                return NotFound();
            }

            user.Bio = bioModel.Bio;

            context.SaveChanges();

            return Ok();
        }

        private double? GetLongestAirtime(int userId) {
            return context.Jumps
                .Where(row => row.Ride.UserId == userId)
                .Max(i => (double?)i.Airtime);
        }

        private double? GetMilesTravelled(int userId, DateTime? dateTime = null) {
            var query = context.Rides
                .Where(row => row.UserId == userId);

            if (dateTime != null) {
                query = query
                    .Where(row => row.StartUtc > dateTime.Value.Date);
            }

            return query.Sum(i => (double?)i.DistanceMiles);
        }

        private double? GetTopSpeedMph(int userId) {
            return context.Rides
                .Where(row => row.UserId == userId)
                .Max(i => (double?)i.MaxSpeedMph);
        }

        private IList<AchievementDto> GetAchievements(int userId) {
            var achievements = new List<AchievementDto>();
            achievements.AddRange(GetSpeedAchievements(userId));
            achievements.AddRange(GetJumpAchievements(userId));
            achievements.AddRange(GetDistanceAchievements(userId));

            return achievements;
        }

        private IEnumerable<AchievementDto> GetSpeedAchievements(int userId) {
            return context.UserSpeedAchievements
                .Where(row => row.UserId == userId)
                .OrderBy(row => row.SpeedAchievement.MinMph)
                .Select(row => new AchievementDto {
                    AchievementId = row.SpeedAchievementId,
                    Name = row.SpeedAchievement.Name,
                })
                .Distinct()
                .ToList();
        }

        private IEnumerable<AchievementDto> GetJumpAchievements(int userId) {
            return context.UserJumpAchievements
                .Where(row => row.UserId == userId)
                .OrderBy(row => row.JumpAchievement.MinAirtime)
                .Select(row => new AchievementDto {
                    AchievementId = row.JumpAchievementId,
                    Name = row.JumpAchievement.Name,
                })
                .Distinct()
                .ToList();
        }

        private IEnumerable<AchievementDto> GetDistanceAchievements(int userId) {
            return context.UserDistanceAchievements
                .Where(row => row.UserId == userId)
                .OrderBy(row => row.DistanceAchievement.MinDistanceMiles)
                .Select(row => new AchievementDto {
                    AchievementId = row.DistanceAchievementId,
                    Name = row.DistanceAchievement.Name,
                })
                .Distinct()
                .ToList();
        }
    }
}
