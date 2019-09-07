using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MtbMate.Utilities {
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
    }
}
