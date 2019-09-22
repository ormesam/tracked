using System.Collections.Generic;
using MtbMate.Contexts;
using MtbMate.Models;
using MtbMate.Utilities;

namespace MtbMate.Screens.Review {
    public class MapScreenViewModel : ViewModelBase {
        private readonly string title;

        public bool ShowSpeed { get; }
        public IList<MapLocation> Locations { get; }

        public MapScreenViewModel(MainContext context, string title, IList<Location> locations) : base(context) {
            this.title = title;
            ShowSpeed = true;
            Locations = PolyUtils.GetMapLocations(locations);
        }

        public MapScreenViewModel(MainContext context, string title, IList<SegmentLocation> locations) : base(context) {
            this.title = title;
            ShowSpeed = false;
            Locations = PolyUtils.GetMapLocations(locations);
        }

        public override string Title => title;
    }
}
