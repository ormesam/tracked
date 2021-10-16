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
    public class SearchController : ControllerBase {
        private readonly DbFactory dbFactory;

        public SearchController(DbFactory dbFactory) {
            this.dbFactory = dbFactory;
        }

        [HttpGet]
        public ActionResult<IList<UserSearchDto>> Get(string searchText) {
            int userId = this.GetCurrentUserId();

            using (Transaction transaction = dbFactory.CreateReadOnlyTransaction()) {
                using (ModelDataContext context = transaction.CreateDataContext()) {
                    var blockedByCurrentUserIds = context.UserBlocks
                        .Where(row => row.UserId == userId)
                        .Select(row => row.BlockUserId)
                        .ToList();

                    var blockedByUserIds = context.UserBlocks
                        .Where(row => row.BlockUserId == userId)
                        .Select(row => row.UserId)
                        .ToList();

                    var allBlocked = blockedByCurrentUserIds.Concat(blockedByUserIds);

                    var users = context.Users
                        .Where(row => row.UserId != userId)
                        .Where(row => !Enumerable.Contains(allBlocked, row.UserId))
                        .Where(row => row.Name.Contains(searchText))
                        .OrderBy(row => row.Name.IndexOf(searchText))
                        .Select(row => new UserSearchDto {
                            UserId = row.UserId,
                            UserName = row.Name,
                            UserProfileImageUrl = row.ProfileImageUrl,
                            JoinedUtc = row.CreatedUtc,
                        })
                        .ToList();

                    return Ok(users);
                }
            }
        }
    }
}
