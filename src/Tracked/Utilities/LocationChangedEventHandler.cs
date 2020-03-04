using Tracked.Models;

namespace Tracked.Utilities {
    public delegate void LocationChangedEventHandler(LocationChangedEventArgs e);

    public class LocationChangedEventArgs {
        public Location Location { get; set; }
    }
}
