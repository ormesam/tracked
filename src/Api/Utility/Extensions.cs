using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;

namespace Api.Utility {
    public static class Extensions {
        public static int GetCurrentUserId(this ControllerBase controller) {
            if (int.TryParse(controller.User.Identity.Name, out int userId)) {
                return userId;
            }

            throw new Exception("Not logged in");
        }

        public static bool IsCurrentUserAdmin(this ControllerBase controller) {
            var identity = controller.User.Identity as ClaimsIdentity;

            if (identity != null) {
                IList<Claim> claims = identity.Claims.ToList();
                var claim = claims.SingleOrDefault(i => i.Type == nameof(UserDto.IsAdmin));

                if (claim != null) {
                    return bool.Parse(claim.Value);
                }
            }

            return false;
        }
    }
}
