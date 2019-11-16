using System;
using Microsoft.AspNetCore.Mvc;

namespace Api.Utility {
    public static class Extensions {
        public static int GetCurrentUserId(this ControllerBase controller) {
            if (int.TryParse(controller.User.Identity.Name, out int userId)) {
                return userId;
            }

            throw new Exception("Not logged in");
        }
    }
}
