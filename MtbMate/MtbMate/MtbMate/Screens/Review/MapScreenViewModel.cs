using System.Collections.Generic;
using System.Linq;
using MtbMate.Contexts;
using MtbMate.Models;

namespace MtbMate.Screens.Review {
    public class MapScreenViewModel : ViewModelBase {
        private readonly string title;

        public bool ShowSpeed { get; }
        public IList<Location> Locations { get; }

        public MapScreenViewModel(MainContext context, string title, IList<Location> locations) : base(context) {
            this.title = title;
            ShowSpeed = true;
            Locations = locations;
        }

        public MapScreenViewModel(MainContext context, string title, IList<LatLng> locations) : base(context) {
            this.title = title;
            ShowSpeed = false;

            Locations = locations
                .Select(i => new Location {
                    LatLong = i,
                })
                .ToList();
        }

        public override string Title => title;
    }
}
