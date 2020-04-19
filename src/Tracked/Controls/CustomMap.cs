using Xamarin.Forms.Maps;

namespace Tracked.Controls {
    public class CustomMap : Map {
        public bool IsReadOnly { get; }

        public CustomMap(MapSpan region, bool isReadOnly) : base(region) {
            IsReadOnly = isReadOnly;
        }
    }
}
