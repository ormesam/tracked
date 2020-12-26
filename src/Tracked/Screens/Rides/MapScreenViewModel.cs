using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shared.Dtos;
using Tracked.Contexts;
using Tracked.Models;
using Tracked.Utilities;
using Xamarin.Forms.Maps;

namespace Tracked.Screens.Rides {
    public class MapScreenViewModel : RideMapViewModelBase {
        private readonly RideDto ride;
        private MapPolyline selectedTrailLine;

        public MapScreenViewModel(MainContext context, RideDto ride)
            : base(context) {

            this.ride = ride;
        }

        public override RideDto Ride => ride;

        public IList<TrailAttemptDto> TrailAttempts => ride.TrailAttempts;

        public void HighlightTrail(TrailAttemptDto attempt) {
            if (selectedTrailLine != null) {
                RemovePolyline(selectedTrailLine);
            }

            selectedTrailLine = new MapPolyline {
                StrokeColor = Color.Blue,
                StrokeWidth = 20f,
                ZIndex = 0,
                Positions = ride.Locations
                    .Where(i => i.Timestamp >= attempt.StartUtc)
                    .Where(i => i.Timestamp <= attempt.EndUtc)
                    .ToList(),
            };

            CreatePolyline(selectedTrailLine);

            var midpoint = selectedTrailLine.Positions.Midpoint();

            GoToLocation(midpoint, Distance.FromMiles(.25));
        }
    }
}
