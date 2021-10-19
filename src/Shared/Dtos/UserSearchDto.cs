using System;

namespace Shared.Dtos {
    public class UserSearchDto {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public DateTime JoinedUtc { get; set; }

        public string JoinedDisplay => $"Member since {JoinedUtc:MMMM dd, yyyy}";
    }
}
