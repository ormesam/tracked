using Shared.Dtos;

namespace Tracked.Utilities {
    public delegate void LocationChangedEventHandler(LocationChangedEventArgs e);

    public class LocationChangedEventArgs {
        public RideLocationDto Location { get; set; }
    }
}
