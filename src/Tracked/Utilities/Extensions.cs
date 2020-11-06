using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shared.Dtos;
using Tracked.Models;
using Xamarin.Forms.Maps;

namespace Tracked.Utilities {
    public static class Extensions {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> list) {
            return new ObservableCollection<T>(list);
        }

        public static Position Midpoint(this IList<MapLocation> list) {
            var lat = list.Average(i => i.Latitude);
            var lon = list.Average(i => i.Longitude);

            return new Position(lat, lon);
        }

        public static double GetTime(this IList<AccelerometerReadingDto> readings) {
            if (!readings.Any()) {
                return 0;
            }

            return (readings.Select(i => i.Timestamp).Max() - readings.Select(i => i.Timestamp).Min()).TotalSeconds;
        }
    }
}
