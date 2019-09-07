using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MtbMate.Utilities {
    public static class Extensions {
        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> list) {
            return new ObservableCollection<T>(list);
        }
    }
}
