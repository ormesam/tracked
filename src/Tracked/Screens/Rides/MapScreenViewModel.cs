using System.Collections.Generic;
using Shared.Dtos;
using Tracked.Contexts;
using Xamarin.Forms.Maps;

namespace Tracked.Screens.Rides {
    public class MapScreenViewModel : RideMapViewModelBase {
        private readonly RideDto ride;
        private Polyline selectedTrailLine;

        public MapScreenViewModel(MainContext context, RideDto ride)
            : base(context) {

            this.ride = ride;
        }

        public override RideDto Ride => ride;

        public IList<TrailAttemptDto> TrailAttempts => ride.TrailAttempts;

        public void HighlightTrail(TrailAttemptDto attempt) {
            //if (selectedTrailLine != null) {
            //    Map.MapElements.Remove(selectedTrailLine);
            //}

            //selectedTrailLine = new Polyline();

            //var latLngs = ride.Locations
            //    .Where(i => i.Timestamp >= attempt.StartUtc)
            //    .Where(i => i.Timestamp <= attempt.EndUtc);


            //foreach (var latLng in latLngs) {
            //    selectedTrailLine.Geopath.Add(new Position(latLng.Latitude, latLng.Longitude));
            //}

            //selectedTrailLine.StrokeColor = Color.Blue;
            //selectedTrailLine.StrokeWidth = 20f;

            //Map.MapElements.Add(selectedTrailLine);

            //var centre = latLngs.Midpoint();
            //Map.MoveToRegion(MapSpan.FromCenterAndRadius(centre, Distance.FromMiles(.25)));
        }
    }
}
