using System.Collections.Generic;
using Tracked.Contexts;
using Tracked.Controls;
using Tracked.Models;

namespace Tracked.Screens.Review {
    public class MapScreenViewModel : MapControlViewModel {
        public MapScreenViewModel(MainContext context, string title, IList<MapLocation> locations, bool showRideFeatures)
            : base(context,
                title,
                locations,
                isReadOnly: false,
                showRideFeatures: showRideFeatures,
                isShowingUser: false,
                goToMapPageOnClick: false,
                canChangeMapType: true) {
        }
    }
}
