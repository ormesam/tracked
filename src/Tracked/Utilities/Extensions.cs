using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Shared.Dtos;

namespace Tracked.Utilities {
    public static class Extensions {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> list) {
            return new ObservableCollection<T>(list);
        }

        public static T Midpoint<T>(this IList<T> list) {
            if (!list.Any()) {
                return default;
            }

            int midpoint = (list.Count - 1) / 2;

            return list[midpoint];
        }

        public static decimal GetTime(this IList<AccelerometerReadingDto> readings) {
            if (!readings.Any()) {
                return 0;
            }

            return (decimal)(readings.Select(i => i.Timestamp).Max() - readings.Select(i => i.Timestamp).Min()).TotalSeconds;
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
