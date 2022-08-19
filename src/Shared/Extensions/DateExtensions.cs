using System;

namespace Shared.Extensions {
    public static class DateExtensions {
        public static string ToReadableString(this DateTime dateTime) {
            var time = dateTime.ToLocalTime();
            var defaultFormat = $"{time:MMMM dd, yyyy} at {time:HH:mm}";

            if (time > DateTime.Now) {
                return defaultFormat;
            }

            var difference = DateTime.Now - time;

            if (difference < TimeSpan.FromSeconds(5)) {
                return "Just now";
            }

            if (difference < TimeSpan.FromMinutes(1)) {
                return $"{difference.Seconds}s ago";
            }

            if (difference < TimeSpan.FromHours(1)) {
                return $"{difference.Minutes}m ago";
            }

            if (time.Date == DateTime.Today) {
                return "Today at " + time.ToString("HH:mm");
            }

            if (time.Date == DateTime.Today.AddDays(-1)) {
                return "Yesterday at " + time.ToString("HH:mm");
            }

            return defaultFormat;
        }
    }
}
