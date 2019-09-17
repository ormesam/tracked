using MtbMate.Models;

namespace MtbMate.Utilities {
    public delegate void LocationChangedEventHandler(LocationChangedEventArgs e);

    public class LocationChangedEventArgs
    {
        public Location Location { get; set; }
    }
}
