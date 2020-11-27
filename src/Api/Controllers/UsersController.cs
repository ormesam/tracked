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
                    Name = row.Name,
                    CreatedUtc = row.CreatedUtc,
                    ProfileImageUrl = row.ProfileImageUrl,
                })
                .SingleOrDefault();

            if (profile == null) {
                return NotFound();
            }

            return profile;
        }
    }
}
