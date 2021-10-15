using System;

namespace Shared.Dtos {
    public class FeedBaseDto {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfileImageUrl { get; set; }
        public DateTime Date { get; set; }

        public string TimeDisplay {
            get {
                var time = Date.ToLocalTime();

                if (time.Date == DateTime.Today) {
                    return "Today at " + time.ToString("HH:mm");
                }

                if (time.Date == DateTime.Today.AddDays(-1)) {
                    return "Yesterday at " + time.ToString("HH:mm");
                }

                return $"{time:MMMM dd, yyyy} at {time:HH:mm}";
            }
        }
    }
}
