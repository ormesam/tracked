using System;

namespace Shared.Dtos {
    public class UserSearchDto {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public DateTime JoinedUtc { get; set; }

        public string JoinedDisplay {
            get {
                var time = JoinedUtc.ToLocalTime();

                if (time.Date == DateTime.Today) {
                    return "Joined today";
                }

                if (time.Date == DateTime.Today.AddDays(-1)) {
                    return "Joined yesterday";
                }

                return $"Joined {time:MMMM dd, yyyy}";
            }
        }
    }
}
