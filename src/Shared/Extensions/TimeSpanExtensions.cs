using System;

namespace Shared.Extensions {
    public static class TimeSpanExtensions {
        public static string ToReadableString(this TimeSpan timeSpan) {
            if (timeSpan < TimeSpan.FromMinutes(1)) {
                return $"{timeSpan.Seconds}s";
            }

            if (timeSpan < TimeSpan.FromHours(1)) {
                return $"{timeSpan.Minutes}m {timeSpan.Seconds}s";
            }

            if (timeSpan < TimeSpan.FromDays(1)) {
                return $"{timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
            }

            return $"{timeSpan.Days}d {timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
        }
    }
}
