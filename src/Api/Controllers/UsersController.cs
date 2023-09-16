using System;
using System.Collections.Generic;
using System.Linq;
using Api.Utility;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace Api.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase {
        private readonly DbFactory dbFactory;

        public UsersController(DbFactory dbFactory) {
            this.dbFactory = dbFactory;
        }

        [HttpGet]
        [Route("{userId}/profile")]
        public ActionResult<ProfileDto> Profile(int userId) {
            int currentUserId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateReadOnlyTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
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

                    profile.IsFollowing = IsFollowing(transaction, currentUserId, userId);
                    profile.IsFollowingCurrentUser = IsFollowing(transaction, userId, currentUserId);
                    profile.LongestAirtime = GetLongestAirtime(transaction, userId);
                    profile.MilesTravelled = GetMilesTravelled(transaction, userId);
                    profile.MilesTravelledThisMonth = GetMilesTravelled(transaction, userId, DateTime.UtcNow.AddDays(-30));
                    profile.TopSpeedMph = GetTopSpeedMph(transaction, userId);
                    profile.Achievements = GetAchievements(transaction, userId);

                    return profile;
                }
            }
        }

        private bool IsFollowing(Transaction transaction, int userId, int followingUserId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                return context.UserFollows
                    .Where(row => row.UserId == userId)
                    .Where(row => row.FollowUserId == followingUserId)
                    .Any();
            }
        }

        [HttpPost]
        [Route("update-bio")]
        public ActionResult UpdateBio(BioChangeDto bioModel) {
            int userId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
                    var user = context.Users.Find(userId);

                    if (user == null) {
                        return NotFound();
                    }

                    user.Bio = bioModel.Bio;

                    context.SaveChanges();
                }

                transaction.Commit();
            }

            return Ok();
        }

        [HttpPost]
        [Route("follow")]
        public ActionResult Follow([FromBody] int followUserId) {
            int userId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
                    var user = context.Users.Find(followUserId);

                    if (user == null) {
                        return NotFound();
                    }

                    context.UserFollows.Add(new UserFollow {
                        UserId = userId,
                        FollowUserId = followUserId,
                        FollowedUtc = DateTime.UtcNow,
                    });

                    context.SaveChanges();
                }

                transaction.Commit();
            }

            return Ok();
        }

        [HttpPost]
        [Route("unfollow")]
        public ActionResult UnFollow([FromBody] int unfollowUserId) {
            using (Transaction transaction = dbFactory.CreateTransaction()) {
                UnfollowUser(transaction, unfollowUserId);

                transaction.Commit();
            }

            return Ok();
        }

        [HttpGet]
        [Route("blocked")]
        public ActionResult<IList<BlockedUserDto>> Blocked() {
            int userId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateReadOnlyTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
                    return context.UserBlocks
                        .Where(row => row.UserId == userId)
                        .Select(row => new BlockedUserDto {
                            UserId = row.BlockUserId,
                            UserName = row.BlockUser.Name,
                            BlockedUtc = row.BlockedUtc,
                        })
                        .ToList();
                }
            }
        }

        [HttpPost]
        [Route("block")]
        public ActionResult Block([FromBody] int blockUserId) {
            int userId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateTransaction()) {
                UnfollowUser(transaction, blockUserId);

                using (ModelDataContext context = transaction.CreateDataContext()) {
                    var user = context.Users.Find(blockUserId);

                    if (user == null) {
                        return NotFound();
                    }

                    context.UserBlocks.Add(new UserBlock {
                        UserId = userId,
                        BlockUserId = blockUserId,
                        BlockedUtc = DateTime.UtcNow,
                    });

                    context.SaveChanges();
                }

                transaction.Commit();
            }

            return Ok();
        }

        [HttpPost]
        [Route("unblock")]
        public ActionResult UnBlock([FromBody] int unblockUserId) {
            int userId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
                    var row = context.UserBlocks
                        .Where(row => row.UserId == userId)
                        .Where(row => row.BlockUserId == unblockUserId)
                        .SingleOrDefault();

                    if (row == null) {
                        return Ok();
                    }

                    context.UserBlocks.Remove(row);

                    context.SaveChanges();
                }

                transaction.Commit();
            }

            return Ok();
        }

        private void UnfollowUser(Transaction transaction, int unfollowUserId) {
            int userId = this.GetCurrentUserId();

            using (ModelDataContext context = transaction.CreateDataContext()) {
                var row = context.UserFollows
                    .Where(row => row.UserId == userId)
                    .Where(row => row.FollowUserId == unfollowUserId)
                    .SingleOrDefault();

                if (row == null) {
                    return;
                }

                context.UserFollows.Remove(row);

                context.SaveChanges();
            }
        }

        private double GetLongestAirtime(Transaction transaction, int userId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                return context.Jumps
                    .Where(row => row.Ride.UserId == userId)
                    .Max(i => (double?)i.Airtime) ?? 0;
            }
        }

        private double GetMilesTravelled(Transaction transaction, int userId, DateTime? dateTime = null) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                var query = context.Rides
                    .Where(row => row.UserId == userId);

                if (dateTime != null) {
                    query = query
                        .Where(row => row.StartUtc > dateTime.Value.Date);
                }

                return query.Sum(i => (double?)i.DistanceMiles) ?? 0;
            }
        }

        private double GetTopSpeedMph(Transaction transaction, int userId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                return context.Rides
                    .Where(row => row.UserId == userId)
                    .Max(i => (double?)i.MaxSpeedMph) ?? 0;
            }
        }

        private IList<AchievementDto> GetAchievements(Transaction transaction, int userId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
                var achievements = new List<AchievementDto>();
                achievements.AddRange(GetSpeedAchievements(transaction, userId));
                achievements.AddRange(GetJumpAchievements(transaction, userId));
                achievements.AddRange(GetDistanceAchievements(transaction, userId));

                return achievements;
            }
        }

        private IEnumerable<AchievementDto> GetSpeedAchievements(Transaction transaction, int userId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
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
        }

        private IEnumerable<AchievementDto> GetJumpAchievements(Transaction transaction, int userId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
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
        }

        private IEnumerable<AchievementDto> GetDistanceAchievements(Transaction transaction, int userId) {
            using (ModelDataContext context = transaction.CreateDataContext()) {
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
}
