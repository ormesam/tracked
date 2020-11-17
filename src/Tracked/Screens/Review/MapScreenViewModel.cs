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
        private Polyline selectedTrailLine;

        public MapScreenViewModel(MainContext context, RideDto ride)
            : base(context, "Ride", PolyUtils.GetMapLocations(ride.Locations, ride.Jumps), isReadOnly: false, showRideFeatures: true, canChangeMapType: true) {

            this.ride = ride;
        }

        public IList<TrailAttemptDto> TrailAttempts => ride.TrailAttempts;

        public void HighlightTrail(TrailAttemptDto attempt) {
            if (selectedTrailLine != null) {
                Map.MapElements.Remove(selectedTrailLine);
            }

            selectedTrailLine = new Polyline();

            var latLngs = ride.Locations
                .Where(i => i.Timestamp >= attempt.StartUtc)
                .Where(i => i.Timestamp <= attempt.EndUtc);


            foreach (var latLng in latLngs) {
                selectedTrailLine.Geopath.Add(new Position(latLng.Latitude, latLng.Longitude));
            }

            selectedTrailLine.StrokeColor = Color.Blue;
            selectedTrailLine.StrokeWidth = 20f;

            Map.MapElements.Add(selectedTrailLine);

            var centre = latLngs.Midpoint();
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(centre, Distance.FromMiles(.25)));
        }
    }
}
