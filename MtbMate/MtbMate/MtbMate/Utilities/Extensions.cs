using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MtbMate.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MtbMate.Utilities {
    public static class Extensions {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> list) {
            return new ObservableCollection<T>(list);
        }

        public static void GoToLocations(this Map map, IList<Location> locations) {
            var firstLocation = locations.Midpoint();

            var pin = new Position(firstLocation.LatLong.Latitude, firstLocation.LatLong.Longitude);

            Device.BeginInvokeOnMainThread(() => {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pin, Distance.FromMiles(0.25)));
            });
        }

        public static T Midpoint<T>(this IList<T> list) {
            if (!list.Any()) {
                return default;
            }

            int midpoint = (list.Count - 1) / 2;

            return list[midpoint];
        }

        public static double GetTime(this IList<AccelerometerReading> readings) {
            if (!readings.Any()) {
                return 0;
            }

            return (readings.Select(i => i.Timestamp).Max() - readings.Select(i => i.Timestamp).Min()).TotalSeconds;
        }

        public static IList<T> GetRange<T>(this IEnumerable<T> enumerable, int index, int count) {
            if (!enumerable.Any()) {
                return default;
            }

            var list = enumerable as List<T>;

            if (list == null) {
                list = enumerable.ToList();
            }

            return list.GetRange(index, count);
        }
    }
}
