using System;
using System.Linq;
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
    public class FeedController : ControllerBase {
        private readonly DbFactory dbFactory;

        public FeedController(DbFactory dbFactory) {
            this.dbFactory = dbFactory;
        }

        [HttpGet]
        public ActionResult<FeedWrapperDto> Get() {
            int userId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateReadOnlyTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
                    DateTime cutOff = DateTime.Today.AddMonths(-3);

                    var blockedUsers = context.UserBlocks
                        .Where(row => row.UserId == userId)
                        .Select(row => row.BlockUserId)
                        .ToList();

                    var blockedByUsers = context.UserBlocks
                        .Where(row => row.BlockUserId == userId)
                        .Select(row => row.BlockUserId)
                        .ToList();

                    var allBlockedUsers = blockedUsers
                        .Concat(blockedByUsers)
                        .Distinct();

                    var followedUsers = context.UserFollows
                        .Where(row => row.UserId == userId)
                        .Select(row => row.FollowUserId)
                        .ToList();

                    var allUsers = followedUsers.ToList();
                    allUsers.Add(userId);

                    var follows = context.UserFollows
                        .Where(row => row.UserFollowId != userId)
                        .Where(row => Enumerable.Contains(followedUsers, row.UserId))
                        .Where(row => !Enumerable.Contains(allBlockedUsers, row.UserFollowId))
                        .Where(row => row.FollowedUtc >= cutOff)
                        .Select(row => new FollowFeedDto {
                            UserId = row.UserId,
                            UserName = row.User.Name,
                            UserProfileImageUrl = row.User.ProfileImageUrl,
                            Date = row.FollowedUtc,
                            FollowedUserId = row.FollowUserId,
                            FollowedUserName = row.FollowUser.Name,
                            FollowedUserProfileImageUrl = row.FollowUser.ProfileImageUrl,
                        })
                        .ToList();

                    var medalsByRide = context.TrailAttempts
                        .Where(row => Enumerable.Contains(allUsers, row.UserId))
                        .Where(row => row.Medal != (int)Medal.None)
                        .ToLookup(row => row.RideId, row => (Medal)row.Medal);

                    var rides = context.Rides
                        .Where(row => Enumerable.Contains(allUsers, row.UserId))
                        .Where(row => row.StartUtc >= cutOff)
                        .OrderByDescending(row => row.StartUtc)
                        .Select(row => new RideFeedDto {
                            UserId = row.UserId,
                            UserName = row.User.Name,
                            UserProfileImageUrl = row.User.ProfileImageUrl,
                            RideId = row.RideId,
                            Name = row.Name,
                            Date = row.StartUtc,
                            DistanceMiles = row.DistanceMiles,
                            EndUtc = row.EndUtc,
                            MaxSpeedMph = row.MaxSpeedMph,
                            RouteSvgPath = row.RouteSvgPath,
                            Medals = medalsByRide[row.RideId],
                        })
                        .ToList();

                    return Ok(new FeedWrapperDto {
                        Rides = rides,
                        Follows = follows,
                    });
                }
            }
        }
    }
}
