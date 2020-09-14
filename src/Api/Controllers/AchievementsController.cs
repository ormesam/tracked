using System.Collections.Generic;
using System.Linq;
using Api.Utility;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementsController : ControllerBase {
        private ModelDataContext context;

        public AchievementsController(ModelDataContext context) {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IList<AchievementDto>> Get() {
            int userId = this.GetCurrentUserId();

            var achievements = new List<AchievementDto>();
            achievements.AddRange(GetSpeedAchievements(userId));
            achievements.AddRange(GetJumpAchievements(userId));

            return achievements;
        }

        private IEnumerable<AchievementDto> GetSpeedAchievements(int userId) {
            var userSpeedAchievements = context.UserSpeedAchievement
                .Where(row => row.UserId == userId)
                .Select(row => new {
                    row.SpeedAchievementId,
                    row.RideId,
                    row.Ride.Name,
                    row.Ride.StartUtc,
                })
                .ToLookup(row => row.SpeedAchievementId, row => new RideOverviewDto {
                    RideId = row.RideId,
                    Name = row.Name,
                    StartUtc = row.StartUtc,
                });

            var speedAchievements = context.SpeedAchievement
                .Select(row => new AchievementDto {
                    AchievementId = row.SpeedAchievementId,
                    Name = row.Name,
                })
                .ToList();

            foreach (var speedAchievement in speedAchievements) {
                speedAchievement.Rides = userSpeedAchievements[speedAchievement.AchievementId].ToList();
            }

            return speedAchievements;
        }

        private IEnumerable<AchievementDto> GetJumpAchievements(int userId) {
            var userJumpAchievements = context.UserJumpAchievement
                .Where(row => row.UserId == userId)
                .Select(row => new {
                    row.JumpAchievementId,
                    row.RideId,
                    row.Ride.Name,
                    row.Ride.StartUtc,
                })
                .ToLookup(row => row.JumpAchievementId, row => new RideOverviewDto {
                    RideId = row.RideId,
                    Name = row.Name,
                    StartUtc = row.StartUtc,
                });

            var jumpAchievements = context.JumpAchievement
                .Select(row => new AchievementDto {
                    AchievementId = row.JumpAchievementId,
                    Name = row.Name,
                })
                .ToList();

            foreach (var jumpAchievement in jumpAchievements) {
                jumpAchievement.Rides = userJumpAchievements[jumpAchievement.AchievementId].ToList();
            }

            return jumpAchievements;
        }
    }
}
