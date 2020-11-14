using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Utilities;
using Xamarin.Forms.Maps;

namespace Tracked.Screens.Review {
    public class MapScreenViewModel : MapControlViewModel {
        private readonly RideDto ride;
        private Polyline selectedSegmentLine;

        public MapScreenViewModel(MainContext context, RideDto ride)
            : base(context, "Ride", PolyUtils.GetMapLocations(ride.Locations, ride.Jumps), isReadOnly: false, showRideFeatures: true, canChangeMapType: true) {

            this.ride = ride;
        }

        public IList<SegmentAttemptDto> SegmentAttempts => ride.SegmentAttempts;

        public void HighlightSegment(SegmentAttemptDto attempt) {
            if (selectedSegmentLine != null) {
                Map.MapElements.Remove(selectedSegmentLine);
            }

            selectedSegmentLine = new Polyline();

            var latLngs = ride.Locations
                .Where(i => i.Timestamp >= attempt.StartUtc)
                .Where(i => i.Timestamp <= attempt.EndUtc);


            foreach (var latLng in latLngs) {
                selectedSegmentLine.Geopath.Add(new Position(latLng.Latitude, latLng.Longitude));
            }

            selectedSegmentLine.StrokeColor = Color.Blue;
            selectedSegmentLine.StrokeWidth = 20f;

            Map.MapElements.Add(selectedSegmentLine);

            var centre = latLngs.Midpoint();
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(centre, Distance.FromMiles(.25)));
        }
    }
}
