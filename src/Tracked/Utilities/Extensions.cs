using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shared.Dtos;
using Shared.Interfaces;
using Tracked.Models;

namespace Tracked.Utilities {
    public static class Extensions {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> list) {
            return new ObservableCollection<T>(list);
        }

        public static ILatLng Midpoint(this IEnumerable<RideLocationDto> list) {
            return Midpoint(list.Cast<ILatLng>());
        }

        public static ILatLng Midpoint(this IEnumerable<MapLocation> list) {
            return Midpoint(list.Cast<ILatLng>());
        }

        public static ILatLng Midpoint(this IEnumerable<ILatLng> list) {
            var lat = list.Average(i => i.Latitude);
            var lon = list.Average(i => i.Longitude);

            return new LatLng(lat, lon);
        }

        public static double GetTime(this IList<AccelerometerReadingDto> readings) {
            if (!readings.Any()) {
                return 0;
            }

            return (readings.Select(i => i.Timestamp).Max() - readings.Select(i => i.Timestamp).Min()).TotalSeconds;
        }
    }
}
